using Microsoft.AspNetCore.Mvc;
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
        public JsonResult GetAlgorithmList()
        {
            // Retrieve and return listing
            var list = _algorithmProvider.GetAlgorithmList();
            
            return new JsonResult(
                new AlgorithmListObject
                {
                    AcvVersion = "1.0",
                    AlgorithmList = list
                });
        }

        [HttpGet("{id}")]
        public JsonResult GetAlgorithmProperties(int id)
        {
            var algo = _algorithmProvider.GetAlgorithm(id);
            return algo == null ? new JsonResult($"No algorithm matches provided id: {id}") : new JsonResult(algo);
        }
    }
}