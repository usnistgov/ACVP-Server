using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Models;
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

        [HttpPost]
        public void CreateVendor(Vendor vendor)
        {
            var vendorResult = _vendorProvider.CreateVendor(vendor);
        }

        [HttpGet("{id}")]
        public void GetVendor(int id)
        {
            var vendorResult = _vendorProvider.GetVendor(id);
        }
        
        [HttpPut("{id}")]
        public void UpdateVendor(int id, Vendor vendor)
        {
            var vendorResult = _vendorProvider.UpdateVendor(id, vendor);
        }

        [HttpDelete("{id}")]
        public void DeleteVendor(int id)
        {
            var result = _vendorProvider.DeleteVendor(id);
        }
    }
}