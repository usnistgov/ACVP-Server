using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class DependencyService : IDependencyService
	{
		private readonly IDependencyProvider _dependencyProvider;

		public DependencyService(IDependencyProvider dependencyProvider)
		{
			_dependencyProvider = dependencyProvider;
		}

		public Dependency GetDependency(long id) => _dependencyProvider.GetDependency(id);

		public bool Exists(long id) => _dependencyProvider.Exists(id);

		public (long TotalCount, List<Dependency> Dependencys) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions)
			=> _dependencyProvider.GetFilteredList(orClauses, pagingOptions.Offset, pagingOptions.Limit);
	}
}