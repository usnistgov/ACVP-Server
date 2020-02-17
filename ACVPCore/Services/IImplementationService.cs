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
		List<Implementation> ListImplementations(long pageSize, long pageNumber);
		bool ImplementationIsUsed(long implementationID);
		ImplementationResult Update(ImplementationUpdateParameters parameters);
	}
}