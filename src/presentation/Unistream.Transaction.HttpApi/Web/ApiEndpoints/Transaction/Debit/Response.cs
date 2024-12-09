namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Debit;

public sealed class Response
{
	public DateTime InsertDateTime { get; set; }
	public decimal ClientBalance { get; set; }
}