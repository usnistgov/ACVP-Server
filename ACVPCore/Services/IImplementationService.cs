using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IImplementationService
	{
		ImplementationResult Create(ImplementationCreateParameters parameters);
		DeleteResult Delete(long implementationID);
		bool ImplementationIsUsed(long implementationID);
		ImplementationResult Update(ImplementationUpdateParameters parameters);
	}
}