using System.Net;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Helpers;
using Web.Public.JsonObjects;
using Web.Public.Providers;

namespace Web.Public.Controllers
{
    [Route("acvp/algorithms")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly IAlgorithmProvider _algorithmProvider;

        public AlgorithmController(IAlgorithmProvider algorithmProvider)
        {
            _algorithmProvider = algorithmProvider;
        }

        [HttpGet]
        public JsonHttpStatusResult GetAlgorithmList()
        {
            // Retrieve and return listing
            var list = _algorithmProvider.GetAlgorithmList();
            return new JsonHttpStatusResult(JsonHelper.BuildVersionedObject(new AlgorithmListObject {AlgorithmList = list}));
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetAlgorithmProperties(int id)
        {
            var algo = _algorithmProvider.GetAlgorithm(id);
            if (algo == null)
            {
                return new JsonHttpStatusResult(
                    JsonHelper.BuildVersionedObject(
                    new ErrorObject
                            {
                                Error = $"No algorithm matches provided id: {id}"
                            }),
                    HttpStatusCode.NotFound);
            }
            
            return new JsonHttpStatusResult(JsonHelper.BuildVersionedObject(algo));
        }
    }
}