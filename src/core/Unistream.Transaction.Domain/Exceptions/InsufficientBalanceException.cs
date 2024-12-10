namespace Unistream.Transaction.Domain.Exceptions;

/// <summary>
///     Ошибка нехватки баланса для применения дебета или отмены кредита
/// </summary>
public class InsufficientBalanceException : DomainException
{
	public InsufficientBalanceException(Guid сlientId, decimal currentAmount)
		: base($"Insufficient balance for {сlientId}, current amount {currentAmount}")
	{
		СlientId = сlientId;
		CurrentAmount = currentAmount;
	}

	public Guid СlientId { get; }
	public decimal CurrentAmount { get; }

	public override string ToString()
	{
		return $"Insufficient balance for {СlientId}, current amount {CurrentAmount}";
	}
}