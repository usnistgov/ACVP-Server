using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Helpers;
using Web.Public.JsonObjects;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/vendors/{vendorId}/addresses")]
    [Authorize]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        
        [HttpGet("{id}")]
        public JsonHttpStatusResult GetAddress(int vendorId, int id)
        {
            var address = _addressService.Get(vendorId, id);
            if (address == null)
            {
                var errorObject = new ErrorObject
                {
                    Error = $"Unable to find address id: {id} under organization id: {vendorId}"
                };
                
                return new JsonHttpStatusResult(JsonHelper.BuildVersionedObject(errorObject), HttpStatusCode.NotFound);
            }
            
            return new JsonHttpStatusResult(JsonHelper.BuildVersionedObject(address));
        }

        [HttpGet]
        public JsonHttpStatusResult GetAddressList(int vendorId)
        {
            // TODO
            return new JsonHttpStatusResult(JsonHelper.BuildVersionedObject(vendorId));
        }
    }
}