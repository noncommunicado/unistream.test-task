using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Unistream.Transaction.Domain.Interfaces;

namespace Unistream.Transaction.Persistence.Entities;

[Table("transaction")]
public class TransactionEntity : ITransaction
{
	public virtual ClientBalanceEntity ClientBalance { get; set; }

	/// <summary>
	///     Баланс до применения транзакции
	/// </summary>
	public decimal CurrentBalance { get; set; }

	/// <summary>
	///     Баланс после применения транзакции
	/// </summary>
	public decimal UpdatedBalance { get; set; }

	[Description("Was transaction reverted")]
	public bool IsReverted { get; set; } = false;

	[Description("Revert date and time")] public DateTime? RevertedDateTime { get; set; }

	[Description("Database creation date and time")]
	public DateTime SysCreated { get; set; } = DateTime.UtcNow;

	public Guid Id { get; set; }

	public Guid ClientId { get; set; }

	public DateTime DateTime { get; set; }

	public decimal Amount { get; set; }
}