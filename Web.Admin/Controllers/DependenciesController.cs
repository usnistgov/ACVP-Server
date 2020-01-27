using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;

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

        //[HttpGet]
        //public WrappedEnumerable<Dependency> GetDependencies()
        //{
        //    return _dependencyService.Get(1, 10).WrapEnumerable();
        //}

        [HttpGet]
        public WrappedEnumerable<Dependency> GetDependencies(long pageSize, long pageNumber)
        {

            Console.WriteLine(pageSize);

            if(pageSize == 0) { pageSize = 10; }
            if(pageNumber == 0) { pageNumber = 1; }

            Console.WriteLine(pageSize);

            return _dependencyService.Get(pageSize, pageNumber).WrapEnumerable();
        }
    }
}