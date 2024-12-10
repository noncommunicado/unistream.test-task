using AutoMapper;
using Unistream.Transaction.Commands.Client;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Client.GetBalance;

public sealed class Response : IMapFrom<GetBalanceQueryResult>
{
	public DateTime BalanceDateTime { get; set; }
	public decimal ClientBalance { get; set; }
}