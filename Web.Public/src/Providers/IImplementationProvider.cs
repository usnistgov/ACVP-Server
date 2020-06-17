using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IImplementationProvider
	{
		Implementation Get(long id);
		bool Exists(long id);
		bool IsUsed(long id);
		(long TotalCount, List<Implementation> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit);
	}
}