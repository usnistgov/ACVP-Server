using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Public.Controllers
{
    [Route("acvp/vendors/{vendorId}/addresses")]
    [Authorize]
    [ApiController]
    public class AddressController : ControllerBase
    {
        [HttpGet("{id}")]
        public void GetAddress(int vendorId, int id)
        {
            
        }

        [HttpGet]
        public void GetAddressList(int vendorId)
        {
            
        }
    }
}