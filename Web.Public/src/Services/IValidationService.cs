
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IValidationService
	{
		Validation GetValidation(long id);
	}
}