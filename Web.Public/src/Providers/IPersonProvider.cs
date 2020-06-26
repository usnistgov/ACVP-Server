using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IPersonProvider
	{
		Person Get(long id);
		bool Exists(long id);
		(long TotalCount, List<Person> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit);
	}
}