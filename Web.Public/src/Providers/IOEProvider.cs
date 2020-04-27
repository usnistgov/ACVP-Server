using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IOEProvider
	{
		OperatingEnvironment Get(long id);
		bool Exists(long id);
		bool IsUsed(long id);
		(long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter);
	}
}