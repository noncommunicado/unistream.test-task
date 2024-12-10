using Unistream.Transaction.Persistence;
using Unistream.Transaction.Persistence.Database;

namespace Unistream.Transaction.HttpApi;

internal sealed class WarmUp
{
	public static async Task RunAsync(WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
		await DatabaseInitializer.InitializeAsync(context);
	}
}