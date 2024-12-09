using System.Globalization;
using FluentValidation;

namespace Unistream.Transaction.HttpApi.Configuration;

internal static class FastEndpointsConfiguration
{
	internal static WebApplicationBuilder AddFastEndpoints(this WebApplicationBuilder webBuilder)
	{
		webBuilder.Services.AddFastEndpoints(opts => { opts.IncludeAbstractValidators = true; });

		// Setup fluent-validation & errors 
		var configCulture = webBuilder.Configuration.GetSection("Culture").Get<string>();
		ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(configCulture ?? "en-US");

		webBuilder.Services.AddAuthentication();
		webBuilder.Services.AddAuthorization();

		return webBuilder;
	}
}