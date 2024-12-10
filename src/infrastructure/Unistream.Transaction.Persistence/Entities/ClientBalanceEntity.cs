using System.ComponentModel.DataAnnotations.Schema;

namespace Unistream.Transaction.Persistence.Entities;

[Table("client_balance")]
public class ClientBalanceEntity
{
	public Guid ClientId { get; set; }
	public decimal Balanace { get; set; }
	public DateTime UpdateDateTime { get; set; }

	public virtual ICollection<TransactionEntity> Transactions { get; set; }
}