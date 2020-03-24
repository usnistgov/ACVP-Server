using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Helpers;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/vendors")]
    [Authorize]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        
        [HttpGet]
        public void GetVendorList()
        {
            var result = _organizationService.GetList(new OrganizationListParameters());
        }

        [HttpGet("{id}")]
        public void GetVendor(long id)
        {
            var result = _organizationService.Get(id);
        }
        
        [HttpPost]
        public void CreateVendor(Organization organization)
        {
            var jsonBody = RequestHelper.GetJsonFromBody(Request.Body);
            var result = _organizationService.Create(null);
        }

        [HttpPut("{id}")]
        public void UpdateVendor(long id)
        {
            var jsonBody = RequestHelper.GetJsonFromBody(Request.Body);
            var result = _organizationService.Update(null);
        }

        [HttpDelete("{id}")]
        public void DeleteVendor(long id)
        {
            var jsonBody = RequestHelper.GetJsonFromBody(Request.Body);
            var result = _organizationService.Delete(id);
        }
    }
}