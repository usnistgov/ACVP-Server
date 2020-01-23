using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACVPCore.Models;
using ACVPCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependenciesController : ControllerBase
    {
        private readonly IDependencyService _dependencyService;

        public DependenciesController(
           IDependencyService dependencyService)
        {
            _dependencyService = dependencyService;
        }

        [HttpGet("{dependencyId}")]
        public Dependency GetDependency(long dependencyId)
        {
            return _dependencyService.Get(dependencyId);
        }
    }
}