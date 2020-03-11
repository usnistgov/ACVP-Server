using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using System.Collections.Generic;

namespace ACVPCore.Services
{
	public interface IImplementationService
	{
		ImplementationResult Create(ImplementationCreateParameters parameters);
		DeleteResult Delete(long implementationID);
		Implementation Get(long implementationID);
		PagedEnumerable<Implementation> ListImplementations(ImplementationListParameters param);
		bool ImplementationIsUsed(long implementationID);
		bool ImplementationExists(long implementationID);
		ImplementationResult Update(ImplementationUpdateParameters parameters);
		Result AddContact(long implementationID, long contactID, int orderIndex);
	}
}