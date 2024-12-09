namespace Unistream.Transaction.Domain.Models.Transaction;

public sealed class DebitTransaction: ITransaction
{
	public Guid Id { get; set; }
	public Guid ClientId { get; set; }
	public DateTime DateTime { get; set; }
	public decimal Amount { get; set; }
}