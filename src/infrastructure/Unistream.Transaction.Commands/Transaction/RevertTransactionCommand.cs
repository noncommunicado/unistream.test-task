using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unistream.Transaction.Domain.Exceptions;
using Unistream.Transaction.Domain.Interfaces;
using Unistream.Transaction.Persistence.Database;
using Unistream.Transaction.Services;

namespace Unistream.Transaction.Commands.Transaction;

public sealed record RevertTransactionCommandResult(DateTime RevertDateTime, decimal ClientBalance);

public sealed record RevertTransactionCommand(Guid TransactionId) : IRequest<RevertTransactionCommandResult>;

public sealed class
	RevertTransactionCommandHandler : IRequestHandler<RevertTransactionCommand, RevertTransactionCommandResult>
{
	private readonly ILockerProvider<Guid> _clientLocker;
	private readonly MainDbContext _context;
	private readonly IMapper _mapper;
	private readonly ILockerProvider<Guid> _transactionLocker;

	public RevertTransactionCommandHandler(
		MainDbContext context,
		[FromKeyedServices(ServiceKeys.Transaction)]
		ILockerProvider<Guid> transactionLocker,
		[FromKeyedServices(ServiceKeys.Client)]
		ILockerProvider<Guid> clientLocker,
		IMapper mapper)
	{
		_context = context;
		_clientLocker = clientLocker;
		_transactionLocker = transactionLocker;
		_mapper = mapper;
	}

	public async Task<RevertTransactionCommandResult> Handle(RevertTransactionCommand request, CancellationToken ct)
	{
		var opDateTime = DateTime.UtcNow;

		// 1. lock transaction 
		using var _ = await _transactionLocker.LockAsync(request.TransactionId, ct);

		// get transaction if presented
		var existed = await _context.Transactions
			.AsNoTracking()
			.Select(x => new {
				x.Id, x.CurrentBalance,
				x.UpdatedBalance, x.ClientId,
				x.IsReverted, x.RevertedDateTime
			})
			.SingleOrDefaultAsync(x => x.Id == request.TransactionId);
		if (existed is null)
			throw new TransactionNotFoundException(request.TransactionId);
		if (existed is {IsReverted: true})
			// не уверен - какую цифру нужно отдавать в балансе после отката транзакции,
			// так что отдам точно не правильную в ситуации с уже произведенным откатом.
			// а в ситуации со свежим - верну текущий баланс клиента после отката (это далее)
			return new RevertTransactionCommandResult(existed.RevertedDateTime!.Value, existed.CurrentBalance);

		// разность между транзакциями, которую необходимо вычесть для revert
		var amountDelta = existed.UpdatedBalance - existed.CurrentBalance;

		// 2. lock client for consistense balance 
		using var __ = await _clientLocker.LockAsync(existed.ClientId, ct);

		// check for Insufficient Balance
		var clientBalance = await _context.Balances
			.SingleAsync(x => x.ClientId == existed.ClientId, ct);
		var balanceAfterRevert = clientBalance.Balanace - amountDelta;
		if (balanceAfterRevert < 0)
			throw new InsufficientBalanceException(existed.ClientId, clientBalance.Balanace);

		using var dbTransaction = await _context.Database.BeginTransactionAsync(ct);
		try {
			// да - тут нужно передалть на хранимую процедуру или на расширенный вызов sql из Dapper
			// не укладывается в рамки задачи

			await _context.Transactions
				.Where(x => x.Id == request.TransactionId)
				.ExecuteUpdateAsync(x =>
					x.SetProperty(p => p.IsReverted, true)
						.SetProperty(p => p.RevertedDateTime, opDateTime), ct);

			clientBalance.Balanace = balanceAfterRevert;
			await _context.SaveChangesAsync(ct);
			await dbTransaction.CommitAsync(ct);
		}
		catch {
			await dbTransaction.RollbackAsync(ct);
			throw;
		}

		return new RevertTransactionCommandResult(opDateTime, clientBalance.Balanace);
	}
}