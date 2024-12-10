using AutoMapper;
using Unistream.Transaction.Commands.Transaction;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Revert;

public sealed class Response : IMapFrom<RevertTransactionCommandResult>
{
	public DateTime RevertDateTime { get; set; }
	public decimal ClientBalance { get; set; }
}