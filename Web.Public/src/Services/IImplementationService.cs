using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IImplementationService
	{
		(long TotalCount, List<Implementation> Implementations) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter);
	}
}