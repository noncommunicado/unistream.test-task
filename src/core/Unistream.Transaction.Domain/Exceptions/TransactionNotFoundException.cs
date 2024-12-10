namespace Unistream.Transaction.Domain.Exceptions;

public class TransactionNotFoundException : DomainException
{
	public TransactionNotFoundException(Guid transactionId) : base($"Transaction not found: {transactionId}")
	{
		TransactionId = transactionId;
	}

	public Guid TransactionId { get; }

	public override string ToString()
	{
		return $"Transaction not found: {TransactionId}";
	}
}