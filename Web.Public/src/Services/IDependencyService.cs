using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IDependencyService
	{
		Dependency GetDependency(long id);
		bool Exists(long id);
		(long TotalCount, List<Dependency> Dependencys) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions);
	}
}