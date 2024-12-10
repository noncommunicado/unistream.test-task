using AutoMapper;
using Unistream.Transaction.Commands.Transaction;
using Unistream.Transaction.Domain.Interfaces;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Debit;

public sealed class Request : ITransaction, IMapTo<CreateTransactionCommand>
{
	public Guid Id { get; set; }
	public Guid ClientId { get; set; }
	public DateTime DateTime { get; set; }
	public decimal Amount { get; set; }
}