using Microsoft.Extensions.DependencyInjection;
using Unistream.Transaction.Domain.Interfaces;
using Unistream.Transaction.Services.ClientSyncContext;

namespace Unistream.Transaction.Services;

public static class DependencyInjection
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		// разделение кэшей транзакций и клиентов,
		// потому что GUID может пересечься при пользовательском вводе
		services.AddKeyedSingleton<ILockerProvider<Guid>, InMemoryLockerProvider>(ServiceKeys.Transaction);
		services.AddKeyedSingleton<ILockerProvider<Guid>, InMemoryLockerProvider>(ServiceKeys.Client);
		return services;
	}
}