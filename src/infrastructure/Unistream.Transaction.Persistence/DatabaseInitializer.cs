using Microsoft.EntityFrameworkCore;
using Serilog;
using Unistream.Transaction.Domain.Static;
using Unistream.Transaction.Persistence.Database;
using Unistream.Transaction.Persistence.Entities;

namespace Unistream.Transaction.Persistence;

public class DatabaseInitializer
{
	public static async Task InitializeAsync(MainDbContext context)
	{
		await context.Database.MigrateAsync();
		await InitClientTransactionsAsync(context);
	}

	private static async Task InitClientTransactionsAsync(MainDbContext context)
	{
		if (await context.Balances
			    .Where(x => x.ClientId == SharedStatics.ClientIdExample)
			    .AnyAsync())
			return;

		await context.Balances.AddAsync(new ClientBalanceEntity {
			ClientId = SharedStatics.ClientIdExample,
			Balanace = 300,
			UpdateDateTime = new DateTime(2024, 10, 10, 14, 10, 0)
		});
		await context.Transactions.AddRangeAsync(new TransactionEntity {
			ClientId = SharedStatics.ClientIdExample,
			Amount = 100,
			DateTime = new DateTime(2024, 10, 10, 14, 0, 0),
			Id = SharedStatics.TransactionIdExample
		}, new TransactionEntity {
			Id = Guid.NewGuid(),
			ClientId = SharedStatics.ClientIdExample,
			Amount = 200,
			DateTime = new DateTime(2024, 10, 10, 14, 10, 0)
		});
		await context.SaveChangesAsync();
		Log.Information("Initialized client transactions");
	}
}