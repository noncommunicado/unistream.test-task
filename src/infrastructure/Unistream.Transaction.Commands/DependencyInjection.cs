using Microsoft.Extensions.DependencyInjection;

namespace Unistream.Transaction.Commands;

public static class DependencyInjection
{
	public static IServiceCollection AddCommands(this IServiceCollection services)
	{
		services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly); });
		return services;
	}
}