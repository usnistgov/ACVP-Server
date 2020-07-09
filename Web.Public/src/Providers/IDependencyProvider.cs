using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IDependencyProvider
	{
		Dependency GetDependency(long id);
		bool Exists(long id);
		(long TotalCount, List<Dependency> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit);
	}
}