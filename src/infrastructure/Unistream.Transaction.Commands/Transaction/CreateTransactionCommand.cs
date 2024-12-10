using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unistream.Transaction.Domain.Exceptions;
using Unistream.Transaction.Domain.Interfaces;
using Unistream.Transaction.Domain.SharedModels;
using Unistream.Transaction.Persistence.Database;
using Unistream.Transaction.Persistence.Entities;
using Unistream.Transaction.Services;

namespace Unistream.Transaction.Commands.Transaction;

public sealed record CreateTransactionCommand
	: ITransaction, IMapTo<TransactionEntity>, IRequest<InsertCommandResult>
{
	public Guid Id { get; set; }
	public Guid ClientId { get; set; }
	public DateTime DateTime { get; set; }
	public decimal Amount { get; set; }
}

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, InsertCommandResult>
{
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	private readonly ILockerProvider<Guid> _syncContext;

	public CreateTransactionCommandHandler(
		MainDbContext context,
		[FromKeyedServices(ServiceKeys.Client)]
		ILockerProvider<Guid> syncContext,
		IMapper mapper)
	{
		_context = context;
		_syncContext = syncContext;
		_mapper = mapper;
	}

	public async Task<InsertCommandResult> Handle(CreateTransactionCommand request, CancellationToken ct)
	{
		var transactionEntity = _mapper.Map<TransactionEntity>(request);
		var opDateTime = DateTime.UtcNow;

		using var _ = await _syncContext.LockAsync(request.ClientId, ct);

		// search and return existed transaction
		var existed = await _context.Transactions
			.AsNoTracking()
			.Select(x => new {x.Id, CurrentBalance = x.UpdatedBalance, x.SysCreated})
			.SingleOrDefaultAsync(x => x.Id == request.Id);
		if (existed is not null)
			return new InsertCommandResult(existed.CurrentBalance, existed.SysCreated);

		// check for Insufficient Balance
		var currentBalance = await _context.Balances.AsNoTracking()
			.SingleOrDefaultAsync(x => x.ClientId == request.ClientId, ct);
		if ((currentBalance is null && request.Amount < 0)
		    || (currentBalance is not null && currentBalance.Balanace + request.Amount < 0)
		   )
			throw new InsufficientBalanceException(request.ClientId, currentBalance?.Balanace ?? 0);

		return await ProccessTransactionAsync(request, currentBalance, opDateTime, transactionEntity, ct);
	}

	private async Task<InsertCommandResult> ProccessTransactionAsync(
		CreateTransactionCommand request,
		ClientBalanceEntity? clientBalance,
		DateTime opDateTime,
		TransactionEntity transactionEntity,
		CancellationToken ct = default
	)
	{
		using var transaction = await _context.Database.BeginTransactionAsync(ct);
		try {
			// todo: replace this peace of code with OUTPUT SQL command or stored proc, or some updatable cache 
			if (clientBalance is null) {
				clientBalance = new ClientBalanceEntity {
					ClientId = request.ClientId,
					Balanace = request.Amount,
					UpdateDateTime = opDateTime
				};
				await _context.Balances.AddAsync(clientBalance, ct);
				await _context.SaveChangesAsync(ct);
				transactionEntity.CurrentBalance = 0;
				transactionEntity.UpdatedBalance = request.Amount;
			}
			else {
				transactionEntity.CurrentBalance = clientBalance.Balanace;
				clientBalance.Balanace += request.Amount;
				transactionEntity.UpdatedBalance = clientBalance.Balanace;
				await _context.Balances
					.Where(x => x.ClientId == request.ClientId)
					.ExecuteUpdateAsync(x =>
						x.SetProperty(
								p => p.Balanace,
								v => clientBalance.Balanace)
							.SetProperty(p => p.UpdateDateTime, _ => opDateTime), ct);
			}

			transactionEntity.SysCreated = opDateTime;
			await _context.AddAsync(transactionEntity, ct);
			await _context.SaveChangesAsync(ct);
			await transaction.CommitAsync(ct);
			return new InsertCommandResult(clientBalance.Balanace, opDateTime);
		}
		catch {
			await transaction.RollbackAsync(ct);
			throw;
		}
	}
}