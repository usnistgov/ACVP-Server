using System.Net;
using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Providers;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/algorithms")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly IAlgorithmProvider _algorithmProvider;
        private readonly IJsonWriterService _jsonWriter;

        public AlgorithmController(IAlgorithmProvider algorithmProvider, IJsonWriterService jsonWriter)
        {
            _algorithmProvider = algorithmProvider;
            _jsonWriter = jsonWriter;
        }

        [HttpGet]
        public JsonHttpStatusResult GetAlgorithmList()
        {
            // Retrieve and return listing
            var list = _algorithmProvider.GetAlgorithmList();
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new AlgorithmListObject {AlgorithmList = list}));
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetAlgorithmProperties(int id)
        {
            var algo = _algorithmProvider.GetAlgorithm(id);
            if (algo == null)
            {
                return new JsonHttpStatusResult(
                    _jsonWriter.BuildVersionedObject(
                    new ErrorObject
                            {
                                Error = $"No algorithm matches provided id: {id}"
                            }),
                    HttpStatusCode.NotFound);
            }
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(algo));
        }
    }
}