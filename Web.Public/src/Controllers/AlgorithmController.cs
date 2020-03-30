using System.Net;
using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/algorithms")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly IAlgorithmService _algoService;
        private readonly IJsonWriterService _jsonWriter;

        public AlgorithmController(IAlgorithmService algoService, IJsonWriterService jsonWriter)
        {
            _algoService = algoService;
            _jsonWriter = jsonWriter;
        }

        [HttpGet]
        public JsonHttpStatusResult GetAlgorithmList()
        {
            // Retrieve and return listing
            var list = _algoService.GetAlgorithmList();
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new AlgorithmListObject {AlgorithmList = list}));
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetAlgorithmProperties(int id)
        {
            var algo = _algoService.GetAlgorithm(id);
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