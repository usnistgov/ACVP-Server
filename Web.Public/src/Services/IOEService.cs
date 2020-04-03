using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IOEService
	{
		OperatingEnvironment GetOE(long id);
		(long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
	}
}