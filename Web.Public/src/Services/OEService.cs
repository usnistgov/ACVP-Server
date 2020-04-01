using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class OEService : IOEService
	{
		private readonly IOEProvider _oeProvider;

		public OEService(IOEProvider oeProvider)
		{
			_oeProvider = oeProvider;
		}

		public (long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter)
			=> _oeProvider.GetFilteredList(filter, pagingOptions.Offset, pagingOptions.Limit, orDelimiter, andDelimiter);
	}
}