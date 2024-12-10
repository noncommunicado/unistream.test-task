using MediatR;
using Unistream.Transaction.Commands.Transaction;
using Unistream.Transaction.Domain.Static;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Credit;

public sealed class Endpoint : Endpoint<Request, Response>
{
	public IMapper Mapper { get; set; }
	public IMediator Mediator { get; set; }

	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("credit");
		Validator<Validator>();
		Summary(s => {
			s.Summary = "Credit amount to client account";
			s.ExampleRequest = new Request {
				Id = SharedStatics.TransactionIdExample,
				ClientId = SharedStatics.ClientIdExample,
				Amount = 100,
				DateTime = DateTime.UtcNow
			};
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var command = Mapper.Map<CreateTransactionCommand>(req);
		var result = await Mediator.Send(command, ct);
		await SendOkAsync(Mapper.Map<Response>(result), ct);
	}
}