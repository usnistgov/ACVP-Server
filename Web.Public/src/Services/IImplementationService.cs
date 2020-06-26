using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IImplementationService
	{
		Implementation GetImplementation(long id);
		bool Exists(long id);
		bool IsUsed(long id);
		(long TotalCount, List<Implementation> Implementations) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions);
	}
}