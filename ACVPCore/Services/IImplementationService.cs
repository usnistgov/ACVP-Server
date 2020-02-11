using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IImplementationService
	{
		ImplementationResult Create(ImplementationCreateParameters parameters);
		DeleteResult Delete(long implementationID);
		Implementation Get(long implementationID);
		bool ImplementationIsUsed(long implementationID);
		ImplementationResult Update(ImplementationUpdateParameters parameters);
	}
}