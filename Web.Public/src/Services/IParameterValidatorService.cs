using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IParameterValidatorService
	{
		ParameterValidationResult Validate(TestSessionRegistration registration);
	}
}