using FluentValidation;

namespace Unistream.Transaction.HttpApi.Web.ApiEndpoints.Transaction.Debit;

public class Validator : Validator<Request>
{
	public Validator()
	{
		RuleFor(x => x.Amount)
			.GreaterThan(0);
		RuleFor(x => x.DateTime)
			.NotEmpty()
			.LessThanOrEqualTo(DateTime.UtcNow)
			.WithMessage("Date time must be less than or equal to UTC now");
	}
}