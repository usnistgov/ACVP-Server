using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/vendors/{vendorId}/addresses")]
    [Authorize]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IJsonWriterService _jsonWriter;

        public AddressController(IAddressService addressService, IJsonWriterService jsonWriter)
        {
            _addressService = addressService;
            _jsonWriter = jsonWriter;
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
                
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(errorObject), HttpStatusCode.NotFound);
            }
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(address));
        }

        [HttpGet]
        public JsonHttpStatusResult GetAddressList(int vendorId)
        {
            // TODO
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(vendorId));
        }
    }
}