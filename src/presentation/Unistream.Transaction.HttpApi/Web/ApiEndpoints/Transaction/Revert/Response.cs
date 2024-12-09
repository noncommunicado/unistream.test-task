namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Revert;

public sealed class Response
{
	public DateTime RevertDateTime { get; set; }
	public decimal ClientBalance { get; set; }
}