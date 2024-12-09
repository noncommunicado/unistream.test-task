namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Revert;

public sealed class Endpoint : Endpoint<Request, Response>
{
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("revert");
		Summary(s => { s.Summary = "Revert debit/credit transaction"; });
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}