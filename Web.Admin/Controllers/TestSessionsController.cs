using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestSessionsController : ControllerBase
    {
        private readonly ITestSessionService _testSessionService;

        public TestSessionsController(ITestSessionService testSessionService)
        {
            _testSessionService = testSessionService;
        }

        [HttpGet]
        public WrappedEnumerable<TestSessionLite> Get()
        {
            return _testSessionService.Get().WrapEnumerable();
        }

        [HttpGet("{testSessionId}")]
        public TestSession Get(long testSessionId)
        {
            return _testSessionService.Get(testSessionId);
        }
    }
}