using AutoMapper;
using Unistream.Transaction.Domain.SharedModels;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Credit;

public sealed class Response : IMapFrom<InsertCommandResult>
{
	public DateTime InsertDateTime { get; set; }
	public decimal ClientBalance { get; set; }
}