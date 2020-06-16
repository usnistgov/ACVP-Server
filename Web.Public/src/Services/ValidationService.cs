using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class ValidationService : IValidationService
	{
		private readonly IValidationProvider _validationProvider;

		public ValidationService(IValidationProvider validationProvider)
		{
			_validationProvider = validationProvider;
		}

		public Validation GetValidation(long id) => _validationProvider.GetValidation(id);
	}
}