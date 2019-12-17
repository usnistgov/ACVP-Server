using System.Collections.Generic;
using System.Linq;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class ValidationService : IValidationService
	{
		IValidationProvider _validationProvider;

		public ValidationService(IValidationProvider validationProvider)
		{
			_validationProvider = validationProvider;
		}

		public InsertResult Create(long implementationID, bool isLCAVP = false) => _validationProvider.Insert(implementationID, isLCAVP);

		public List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID) => _validationProvider.GetValidationsForImplementation(implementationID);

		//Get the most recently created ACVP validation ID, or 0 if none exists
		public long GetLatestACVPValidationForImplementation(long implementationID) => GetValidationsForImplementation(implementationID).Where(x => x.ValidationSource == 1).DefaultIfEmpty().Max(x => x.ValidationID);
	}
}
