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

		public (long TotalCount, List<Organization> Organizations) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter)
			=> _organizationProvider.GetFilteredList(filter, pagingOptions.Offset, pagingOptions.Limit, orDelimiter, andDelimiter);
	}
}