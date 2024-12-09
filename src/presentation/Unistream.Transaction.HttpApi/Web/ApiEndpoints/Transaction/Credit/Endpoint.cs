namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Credit;

public sealed class Endpoint : Endpoint<CreditTransaction, Response>
{
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("credit");
		Summary(s => { s.Summary = "Credit amount to client account"; });
	}

	public override async Task HandleAsync(CreditTransaction req, CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}