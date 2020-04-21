using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
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
		PagedEnumerable<OperatingEnvironmentLite> Get(OeListParameters param);
	}
}