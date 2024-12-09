namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Client.GetBalance;

public sealed class Request
{
	[FromQuery] public Guid Id { get; set; }
}