using MediatR;
using Unistream.Transaction.Commands.Transaction;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Revert;

public sealed class Endpoint : EndpointWithoutRequest<Response>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }

	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("revert/{id}");
		Summary(s => { s.Summary = "Revert debit/credit transaction"; });
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var id = Route<Guid>("id");
		var result = await Mediator.Send(new RevertTransactionCommand(id), ct);
		await SendOkAsync(Mapper.Map<Response>(result), ct);
	}
}