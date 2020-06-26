using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IPersonService
	{
		Person Get(long personID);
		bool Exists(long personID);
		(long TotalCount, List<Person> Persons) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions);
	}
}