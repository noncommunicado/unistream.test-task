namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Client.GetBalance;

public sealed class Endpoint : Endpoint<Request, Response>
{
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("balance");
		Summary(s => { s.Summary = "Get client balance"; });
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}