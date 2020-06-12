using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IValidationProvider
	{
		Validation GetValidation(long id);
	}
}