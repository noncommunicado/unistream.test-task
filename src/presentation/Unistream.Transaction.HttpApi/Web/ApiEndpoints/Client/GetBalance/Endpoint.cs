using MediatR;
using Unistream.Transaction.Commands.Client;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Client.GetBalance;

public sealed class Endpoint : EndpointWithoutRequest<Response>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }

	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("balance/{id}");
		Summary(s => { s.Summary = "Get client balance"; });
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var id = Route<Guid>("id");
		var clientBalance = await Mediator.Send(new GetBalanceQuery(id), ct);
		if (clientBalance.HasValue)
			await SendOkAsync(Mapper.Map<Response>(clientBalance.Value), ct);
		else
			ThrowError("client balance not found", 404);
	}
}