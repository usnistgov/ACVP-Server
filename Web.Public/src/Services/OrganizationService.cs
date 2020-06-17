using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly IOrganizationProvider _organizationProvider;

		public OrganizationService(IOrganizationProvider organizationProvider)
		{
			_organizationProvider = organizationProvider;
		}

		public Organization Get(long organizationID) => _organizationProvider.Get(organizationID);

		public bool Exists(long organizationID) => _organizationProvider.Exists(organizationID);

		public bool IsUsed(long organizationID) => _organizationProvider.IsUsed(organizationID);

		public (long TotalCount, List<Organization> Organizations) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions)
			=> _organizationProvider.GetFilteredList(orClauses, pagingOptions.Offset, pagingOptions.Limit);

		public (long TotalCount, List<Person> Contacts) GetContactFilteredList(long organizationId, List<OrClause> orClauses, PagingOptions pagingOptions)
			=> _organizationProvider.GetContactFilteredList(organizationId, orClauses, pagingOptions.Offset, pagingOptions.Limit);
	}
}