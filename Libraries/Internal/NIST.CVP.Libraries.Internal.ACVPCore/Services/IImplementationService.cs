using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
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