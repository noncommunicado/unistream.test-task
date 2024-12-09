using FastEndpoints.Swagger;
using Serilog;
using Unistream.Transaction.HttpApi.Configuration;

try {
	await RunAsync();
}
catch (Exception e) {
	Log.Fatal(e, "Global unhandled exception");
}
finally {
	Log.Warning("Application stop");
	Log.CloseAndFlush();
}


async Task RunAsync()
{
	var builder = WebApplication.CreateBuilder(args);
	builder.AddFastEndpoints()
		.ConfigureSettings()
		.ConfigureSwagger()
		.ConfigureSerilogging();

	var app = builder.Build();
	app.UseRequestLocalization();
	app.UseAuthentication();
	app.UseAuthorization();

	app.UseFastEndpoints(c => {
		c.Errors.UseProblemDetails();
		c.Endpoints.RoutePrefix = "api";
		c.Versioning.Prefix = "v";
		c.Versioning.PrependToRoute = true;
	});
	app.UseSwaggerGen();
	app.UseHttpRequestsLogging();

	await app.RunAsync();
}