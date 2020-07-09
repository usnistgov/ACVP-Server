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

		public OperatingEnvironmentWithDependencies GetOE(long id) => _oeProvider.Get(id);
		public bool Exists(long id) => _oeProvider.Exists(id);

		public bool IsUsed(long id) => _oeProvider.IsUsed(id);

		public (long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions)
			=> _oeProvider.GetFilteredList(orClauses, pagingOptions.Offset, pagingOptions.Limit);

		public List<long> GetForValidation(long validationID) => _oeProvider.GetForValidation(validationID);
	}
}