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
		OperatingEnvironment Get(long dependencyId);
		List<OperatingEnvironmentLite> Get(long pageSize, long pageNumber);
	}
}