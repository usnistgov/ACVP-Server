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
        public JsonResult GetVendorList(int limit, int offset)
        {
            var pagingOptions = new PagingOptions
            {
                Limit = limit,
                Offset = offset
            };
            
            var result = _organizationService.GetList(pagingOptions);
            return new JsonResult("");
        }

        [HttpGet("{id}")]
        public JsonResult GetVendor(long id)
        {
            var result = _organizationService.Get(id);
            return new JsonResult(result);
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