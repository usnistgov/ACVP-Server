using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IImplementationProvider
	{
		(long TotalCount, List<Implementation> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter);
	}
}