using Serilog;
using Serilog.Events;

namespace Unistream.Transaction.HttpApi.Configuration;

internal static class LogConfiguration
{
	internal static WebApplicationBuilder ConfigureSerilogging(this WebApplicationBuilder builder)
	{
		builder.Logging.ClearProviders();
		builder.Host.UseSerilog((_, __, configuration) => {
			configuration
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.Enrich.FromLogContext();
#if DEBUG
			configuration.WriteTo.Logger(c => c.WriteTo.Console(LogEventLevel.Information));
#else
			configuration
				.WriteTo.File(Path.Combine("logs", "_.log"),
					rollingInterval: RollingInterval.Day,
					restrictedToMinimumLevel: LogEventLevel.Information,
					retainedFileCountLimit: 30);
			// todo: elastic serilog sinks setup
#endif
		});
		return builder;
	}

	internal static WebApplication UseHttpRequestsLogging(this WebApplication app)
	{
		app.UseSerilogRequestLogging(options => {
			// Attach additional properties to the request completion event
			options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
				diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme.ToUpper());
				diagnosticContext.Set("ContentLength", httpContext.Request.ContentLength ?? 0);
				diagnosticContext.Set("ContentType", httpContext.Request.ContentType ?? "[none]");
				diagnosticContext.Set("HeadersCount", httpContext.Request.Headers.Count);
				diagnosticContext.Set("CookiesCount", httpContext.Request.Cookies.Count);
				diagnosticContext.Set("TraceIdentifier", httpContext.TraceIdentifier);
			};

			options.MessageTemplate = Environment.NewLine +
			                          "\t{RequestScheme} ({TraceIdentifier}) from {RemoteIpAddress} with {RequestMethod} {RequestPath}" +
			                          Environment.NewLine +
			                          "\tResponded {StatusCode} in {Elapsed:0.0000} ms; Body: {ContentLength} bytes; " +
			                          Environment.NewLine +
			                          "\tContentType: {ContentType}; Headers: {HeadersCount}; Cookies: {CookiesCount}; Forwarded: {Forwarded}.";
			options.IncludeQueryInRequestPath = true;
		});
		return app;
	}
}