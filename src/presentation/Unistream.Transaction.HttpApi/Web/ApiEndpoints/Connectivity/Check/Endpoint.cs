namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Connectivity.Check;

internal sealed class Endpoint : EndpointWithoutRequest
{
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("connectivity/check");
		Summary(s => {
			s.Summary = "Check is api available and provides response";
			s.Responses[StatusCodes.Status200OK] = "OK";
		});
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		await SendOkAsync("OK");
	}
}