using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public interface IDependencyService
	{
		InsertResult InsertAttribute(long dependencyID, string name, string value);
		DeleteResult Delete(long dependencyID);
		Result DeleteAttribute(long attributeID);
		DeleteResult DeleteEvenIfUsed(long dependencyID);
		DependencyResult Create(DependencyCreateParameters dependency);
		DependencyResult Update(DependencyUpdateParameters parameters);
		Dependency Get(long dependencyId);
		PagedEnumerable<Dependency> Get(DependencyListParameters param);
		bool DependencyIsUsed(long dependencyID);
		bool DependencyExists(long dependencyID);
	}
}