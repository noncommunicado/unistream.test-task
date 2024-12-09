namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Connectivity.GenerateError;

internal class Endpoint : EndpointWithoutRequest
{
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("connectivity/error");
		Summary(s => { s.Summary = "Generates a random error"; });
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		ThrowError("Error descriptions message", Random.Shared.Next(400, 503));
	}
}