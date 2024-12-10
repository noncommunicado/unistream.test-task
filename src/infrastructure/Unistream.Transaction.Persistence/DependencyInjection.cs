using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unistream.Transaction.Persistence.Database;

namespace Unistream.Transaction.Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<MainDbContext>(o => {
			o.UseSqlite(connectionString);
			o.UseExceptionProcessor();
			o.UseSnakeCaseNamingConvention();
		});
		return services;
	}
}