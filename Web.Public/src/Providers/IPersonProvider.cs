using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IPersonProvider
	{
		(long TotalCount, List<Person> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter);
	}
}