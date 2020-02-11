using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IImplementationService _implementationService;

        public ProductsController(
           IImplementationService implementationService)
        {
            _implementationService = implementationService;
        }

        [HttpGet("{implementationId}")]
        public Implementation Get(long implementationId)
        {
            return _implementationService.Get(implementationId);
        }
    }
}