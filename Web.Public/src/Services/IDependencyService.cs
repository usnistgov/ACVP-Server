using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IDependencyService
	{
		(long TotalCount, List<Dependency> Dependencys) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
	}
}