using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IOEService
	{
		OperatingEnvironmentWithDependencies GetOE(long id);
		bool Exists(long id);
		bool IsUsed(long id);
		(long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
	}
}