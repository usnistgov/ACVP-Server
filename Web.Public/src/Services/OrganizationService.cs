using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly IOrganizationProvider _organizationProvider;
		private readonly IAddressProvider _addressProvider;

		public OrganizationService(IOrganizationProvider organizationProvider, IAddressProvider addressProvider)
		{
			_organizationProvider = organizationProvider;
			_addressProvider = addressProvider;
		}

		public Organization Get(long organizationID)
		{
			Organization org = _organizationProvider.Get(organizationID);
			org.Addresses = _addressProvider.GetAddressList(organizationID);
			return org;
		}

		public bool Exists(long organizationID) => _organizationProvider.Exists(organizationID);

		public bool IsUsed(long organizationID) => _organizationProvider.IsUsed(organizationID);

		public (long TotalCount, List<Organization> Organizations) GetFilteredList(List<OrClause> orClauses, PagingOptions pagingOptions)
			=> _organizationProvider.GetFilteredList(orClauses, pagingOptions.Offset, pagingOptions.Limit);

		public (long TotalCount, List<Person> Contacts) GetContactFilteredList(long organizationId, List<OrClause> orClauses, PagingOptions pagingOptions)
			=> _organizationProvider.GetContactFilteredList(organizationId, orClauses, pagingOptions.Offset, pagingOptions.Limit);
	}
}