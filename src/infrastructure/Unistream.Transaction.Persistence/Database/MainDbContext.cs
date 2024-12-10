using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;
using Unistream.Transaction.Persistence.Entities;

namespace Unistream.Transaction.Persistence.Database;

public class MainDbContext : DbContext
{
	public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
	public MainDbContext() { }

	public DbSet<TransactionEntity> Transactions { get; set; }
	public DbSet<ClientBalanceEntity> Balances { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder ob)
	{
		ob.UseExceptionProcessor();
		ob.UseSnakeCaseNamingConvention();
		base.OnConfiguring(ob);
	}

	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<TransactionEntity>()
			.HasKey(x => x.Id);
		mb.Entity<TransactionEntity>()
			.HasIndex(x => x.SysCreated);
		mb.Entity<TransactionEntity>()
			.HasIndex(x => x.DateTime);
		mb.Entity<TransactionEntity>()
			.HasOne(x => x.ClientBalance)
			.WithMany(x => x.Transactions)
			.HasForeignKey(x => x.ClientId);

		mb.Entity<ClientBalanceEntity>()
			.HasKey(x => x.ClientId);
	}
}