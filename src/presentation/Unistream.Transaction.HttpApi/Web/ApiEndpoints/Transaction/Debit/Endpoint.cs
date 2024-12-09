namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Debit;

public sealed class Endpoint : Endpoint<DebitTransaction, Response>
{
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("debit");
		Summary(s => { s.Summary = "Debit amount to client account"; });
	}

	public override async Task HandleAsync(DebitTransaction req, CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}