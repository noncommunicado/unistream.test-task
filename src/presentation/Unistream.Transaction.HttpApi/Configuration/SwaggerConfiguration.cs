using FastEndpoints.Swagger;

namespace Unistream.Transaction.HttpApi.Configuration;

internal static class SwaggerConfiguration
{
	internal static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
	{
		builder.Services.SwaggerDocument(o => {
			o.RemoveEmptyRequestSchema = true;
			o.MaxEndpointVersion = 2;
			o.MinEndpointVersion = 1;
			o.ShortSchemaNames = true;
			o.DocumentSettings = s => {
				s.Title = "Unistream Test Task #4";
				s.Version = "v1";
			};

			o.TagDescriptions = t => {
				t["Connectivity"] = "Helper methods for connectivity checks";
			};
		});
		return builder;
	}
}