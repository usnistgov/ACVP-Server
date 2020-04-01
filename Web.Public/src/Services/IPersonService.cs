using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IPersonService
	{
		Person Get(long personID);
		(long TotalCount, List<Person> Persons) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
	}
}