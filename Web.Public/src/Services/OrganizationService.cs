using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
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
        
        public Organization Get(long organizationID)
        {
            return _organizationProvider.Get(organizationID);
        }

        public PagedEnumerable<OrganizationLite> GetList(OrganizationListParameters param)
        {
            return _organizationProvider.GetList(param);
        }
        
        public OrganizationResult Create(OrganizationCreateParameters parameters)
        {
            // Build message and place into queue
            throw new System.NotImplementedException();
        }

        public OrganizationResult Update(OrganizationUpdateParameters parameters)
        {
            // Build message and place into queue
            throw new System.NotImplementedException();
        }

        public DeleteResult Delete(long organizationID)
        {
            // Build message and place into queue
            throw new System.NotImplementedException();
        }
    }
}