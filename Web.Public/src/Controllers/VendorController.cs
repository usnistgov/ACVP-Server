using ACVPCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Helpers;
using Web.Public.Providers;

namespace Web.Public.Controllers
{
    [Route("acvp/vendors")]
    [Authorize]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorProvider _vendorProvider;
        
        public VendorController(IVendorProvider vendorProvider)
        {
            _vendorProvider = vendorProvider;
        }
        
        [HttpGet]
        public void GetVendorList()
        {
            var vendorList = _vendorProvider.GetVendorList();
        }

        [HttpGet("{id}")]
        public void GetVendor(int id)
        {
            var vendorResult = _vendorProvider.GetVendor(id);
        }
        
        [HttpPost]
        public void CreateVendor(Organization organization)
        {
            var jsonBody = RequestHelper.GetJsonFromBody(Request.Body);
            var vendorResult = _vendorProvider.CreateVendor(organization);
        }

        [HttpPut("{id}")]
        public void UpdateVendor(int id)
        {
            var jsonBody = RequestHelper.GetJsonFromBody(Request.Body);
            //var vendorResult = _vendorProvider.UpdateVendor(id, vendor);
        }

        [HttpDelete("{id}")]
        public void DeleteVendor(int id)
        {
            var jsonBody = RequestHelper.GetJsonFromBody(Request.Body);
            var result = _vendorProvider.DeleteVendor(id);
        }
    }
}