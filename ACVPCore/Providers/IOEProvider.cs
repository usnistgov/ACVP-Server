using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IOEProvider
	{
		InsertResult Insert(string name);
		Result InsertDependencyLink(long oeID, long dependencyID);
		Result Update(long oeID, string name);
		Result Delete(long oeID);
		Result DeleteAllDependencyLinks(long oeID);
		Result DeleteDependencyLink(long oeID, long dependencyID);
		List<long> GetDependencyLinks(long oeID);
		bool OEIsUsed(long oeID);
		bool OEExists(long oeID);
		OperatingEnvironment Get(long oeID);
		List<OperatingEnvironmentLite> Get(long pageSize, long pageNumber);
	}
}