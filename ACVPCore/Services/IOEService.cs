using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using System.Collections.Generic;

namespace ACVPCore.Services
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