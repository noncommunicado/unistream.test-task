namespace Unistream.Transaction.HttpApi.Configuration;

internal static class SettingsConfiguration
{
	internal static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder builder)
	{
		builder.Configuration.AddJsonFile(
			Path.Combine("appsettings", "appsettings.json"),
			false,
			false
		);
		builder.Configuration.AddJsonFile(
			Path.Combine("appsettings", $"appsettings.{builder.Environment.EnvironmentName}.json"),
			true,
			false
		);
		return builder;
	}
}