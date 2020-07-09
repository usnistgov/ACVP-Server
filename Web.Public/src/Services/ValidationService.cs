using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class ValidationService : IValidationService
	{
		private readonly IValidationProvider _validationProvider;
		private readonly IOEService _oeService;

		public ValidationService(IValidationProvider validationProvider, IOEService oeService)
		{
			_validationProvider = validationProvider;
			_oeService = oeService;
		}

		public Validation GetValidation(long id)
		{
			Validation validation = _validationProvider.GetValidation(id);
			validation.OEIDs = _oeService.GetForValidation(id);
			return validation;
		}
	}
}