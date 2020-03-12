using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Providers
{
	public interface IDependencyProvider
	{
		Dependency Get(long dependencyID);
		PagedEnumerable<Dependency> Get(DependencyListParameters param);
		List<DependencyAttribute> GetAttributes(long dependencyID);

		Result Delete(long dependencyID);
		Result DeleteAllAttributes(long dependencyID);
		Result DeleteAllOELinks(long dependencyID);
		Result DeleteAttribute(long attributeID);
		Result DeleteOELink(long dependencyID, long oeID);

		InsertResult Insert(string type, string name, string description);
		InsertResult InsertAttribute(long dependencyID, string name, string value);
		Result Update(long dependencyID, string type, string name, string description, bool typeUpdated, bool nameUpdated, bool descriptionUpdated);

		bool DependencyIsUsed(long dependencyID);
		bool DependencyExists(long dependencyID);
	}
}