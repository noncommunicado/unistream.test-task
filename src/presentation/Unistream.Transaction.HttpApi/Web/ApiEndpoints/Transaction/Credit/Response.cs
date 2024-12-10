using AutoMapper;
using Unistream.Transaction.Commands.Transaction;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Credit;

public sealed class Response : IMapFrom<InsertTransactionResult>
{
	public DateTime InsertDateTime { get; set; }
	public decimal ClientBalance { get; set; }
}