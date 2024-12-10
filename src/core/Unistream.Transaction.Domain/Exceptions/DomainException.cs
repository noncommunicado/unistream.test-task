namespace Unistream.Transaction.Domain.Exceptions;

public abstract class DomainException : Exception
{
	protected DomainException(string? message) : base(message) { }
}