using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IOEProvider
	{
		(long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter);
	}
}