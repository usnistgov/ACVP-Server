using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IDependencyProvider
	{
		(long TotalCount, List<Dependency> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter);
	}
}