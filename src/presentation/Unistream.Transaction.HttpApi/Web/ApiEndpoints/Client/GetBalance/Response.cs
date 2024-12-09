namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Client.GetBalance;

public sealed class Response
{
	public DateTime BalanceDateTime { get; set; }
	public decimal ClientBalance { get; set; }
}