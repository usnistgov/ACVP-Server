using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IOEService
	{
		OEResult Create(OECreateParameters oe);
		OEResult Update(OEUpdateParameters parameters);
		DeleteResult Delete(long oeID);
		bool OEIsUsed(long oeID);
		bool OEExists(long oeID);
		OperatingEnvironment Get(long dependencyId);
		PagedEnumerable<OperatingEnvironmentLite> Get(OeListParameters param);
		Result AddDependencyLink(long oeID, long dependencyID);
		Result RemoveDependencyLink(long oeID, long dependencyID);
	}
}