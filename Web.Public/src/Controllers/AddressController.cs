using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/vendors/{vendorId}/addresses")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IJsonWriterService _jsonWriter;

        public AddressController(
            IAddressService addressService,
            IJsonWriterService jsonWriter)
        {
            _addressService = addressService;
            _jsonWriter = jsonWriter;
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetAddress(long vendorId, long id)
        {
            var address = _addressService.Get(vendorId, id);
            if (address == null)
            {
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
                {
                    Error = Request.HttpContext.Request.Path,
                    Context = $"Unable to find address id: {id} under vendor id: {vendorId}."
                }), HttpStatusCode.NotFound);
            }
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(address));
        }

        [HttpGet]
        public JsonHttpStatusResult GetAddressList(long vendorId)
        {
            //Try to read limit and offset, if passed in
            var limit = 0;
            var offset = 0;
            if (Request.Query.TryGetValue("limit", out var stringLimit))
            {
                int.TryParse(stringLimit.First(), out limit);
            }

            if (Request.Query.TryGetValue("offset", out var stringOffset))
            {
                int.TryParse(stringOffset.First(), out offset);
            }

            //If limit was not present, or a garbage value, make it a default
            if (limit <= 0) limit = 20;

            var pagingOptions = new PagingOptions
            {
                Limit = limit,
                Offset = offset
            };

            // Note this has permission to change Limit
            var (totalRecords, addresses) = _addressService.GetAddressList(vendorId, pagingOptions);
            var pagedData =  new PagedResponse<Address>(totalRecords, addresses, $"/acvp/v1/vendors/{vendorId}/addresses", pagingOptions);
				
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(pagedData));
        }
    }
}