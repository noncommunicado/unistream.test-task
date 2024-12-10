using AutoMapper;
using FastEndpoints.Swagger;
using Serilog;
using Unistream.Transaction.Commands;
using Unistream.Transaction.HttpApi;
using Unistream.Transaction.HttpApi.Configuration;
using Unistream.Transaction.Persistence;
using Unistream.Transaction.Services;

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

	builder.Services.AddPersistence(builder.Configuration.GetConnectionString("Main")!);
	builder.Services.AddCommands();
	builder.Services.AddServices();
	builder.Services.AddAllMappings(); // automapper ext

	var app = builder.Build();
	app.UseRequestLocalization();
	app.UseAuthentication();
	app.UseAuthorization();

	app
		.UseDefaultExceptionHandler()
		.UseFastEndpoints(c => {
			c.Errors.UseProblemDetails();
			c.Endpoints.RoutePrefix = "api";
			c.Versioning.Prefix = "v";
			c.Versioning.PrependToRoute = true;
		});
	app.UseSwaggerGen();
	app.UseHttpRequestsLogging();

	await WarmUp.RunAsync(app);
	await app.RunAsync();
}