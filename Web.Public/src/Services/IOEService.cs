using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IOEService
	{
		OperatingEnvironmentWithDependencies GetOE(long id);
		bool Exists(long id);
		bool IsUsed(long id);
		(long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions);
	}
}