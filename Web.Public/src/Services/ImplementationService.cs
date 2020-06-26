using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class ImplementationService : IImplementationService
	{
		private readonly IImplementationProvider _implementationProvider;

		public ImplementationService(IImplementationProvider implementationProvider)
		{
			_implementationProvider = implementationProvider;
		}

		public Implementation GetImplementation(long id) => _implementationProvider.Get(id);
		public bool Exists(long id) => _implementationProvider.Exists(id);
		public bool IsUsed(long id) => _implementationProvider.IsUsed(id);

		public (long TotalCount, List<Implementation> Implementations) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions)
			=> _implementationProvider.GetFilteredList(orClauses, pagingOptions.Offset, pagingOptions.Limit);
	}
}