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
        public string GetAlgorithmProperties(int id)
        {
            var algo = _algorithmProvider.GetAlgorithm(id);
            return "";
            //return algo == null ? new JsonResult($"No algorithm matches provided id: {id}").ToString() : JsonHelper.BuildVersionedObject(algo);
        }
    }
}