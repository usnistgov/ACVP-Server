using System.Collections.Generic;
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
        public List<TestSessionLite> Get()
        {
            return _testSessionService.Get();
        }

        [HttpGet("{testSessionId}")]
        public TestSession Get(long testSessionId)
        {
            return _testSessionService.Get(testSessionId);
        }
    }
}