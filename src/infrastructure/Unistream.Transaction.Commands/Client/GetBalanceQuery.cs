using KutCode.Optionality;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Unistream.Transaction.Domain.Interfaces;
using Unistream.Transaction.Persistence.Database;
using Unistream.Transaction.Services;

namespace Unistream.Transaction.Commands.Client;

public record GetBalanceQueryResult(decimal ClientBalance, DateTime BalanceDateTime);

public record GetBalanceQuery(Guid ClientId) : IRequest<Optional<GetBalanceQueryResult>>;

public sealed class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, Optional<GetBalanceQueryResult>>
{
	private readonly MainDbContext _context;
	private readonly ILockerProvider<Guid> _syncContext;

	public GetBalanceQueryHandler(
		MainDbContext context,
		[FromKeyedServices(ServiceKeys.Client)]
		ILockerProvider<Guid> syncContext
	)
	{
		_context = context;
		_syncContext = syncContext;
	}

	public async Task<Optional<GetBalanceQueryResult>> Handle(GetBalanceQuery request, CancellationToken ct)
	{
		using var _ = await _syncContext.LockAsync(request.ClientId, ct);
		try {
			var entity = await _context.Balances.AsNoTracking()
				.FirstOrDefaultAsync(x => x.ClientId == request.ClientId, ct);
			if (entity is null) return Optional.None<GetBalanceQueryResult>();
			return new GetBalanceQueryResult(entity.Balanace, entity.UpdateDateTime);
		}
		catch (Exception e) {
			Log.Error(e, "Error while getting balance");
			return Optional.None<GetBalanceQueryResult>();
		}
	}
}