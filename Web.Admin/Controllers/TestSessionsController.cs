using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestSessionsController : ControllerBase
    {
        private readonly ITestSessionService _testSessionService;
        private readonly IVectorSetService _vectorSetService;

        public TestSessionsController(
            ITestSessionService testSessionService,
            IVectorSetService vectorSetService)
        {
            _testSessionService = testSessionService;
            _vectorSetService = vectorSetService;
        }

        [HttpGet]
        public WrappedEnumerable<TestSessionLite> GetTestSessions()
        {
            return _testSessionService.Get().WrapEnumerable();
        }

        [HttpGet("{testSessionId}")]
        public TestSession GetTestSessionDetails(long testSessionId)
        {
            return _testSessionService.Get(testSessionId);
        }

        [HttpPost("{testSessionId}")]
        public Result CancelTestSession(long testSessionId)
        {
            return _testSessionService.Cancel(testSessionId);
        }
        
        [HttpPost("vectorSet/{vectorSetId}")]
        public Result CancelVectorSet(long vectorSetId)
        {
            return _vectorSetService.Cancel(vectorSetId);
        }
    }
}