namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Revert;

public sealed class Request
{
	[FromQuery] public Guid Id { get; set; }
}