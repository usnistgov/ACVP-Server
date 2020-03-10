using System.Collections.Generic;
using System.IO;
using ACVPCore.Results;
using ACVPCore.Services;
using LCAVPCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegacyController : ControllerBase
    {
        private readonly ILCAVPSubmissionProcessor _lcavpSubmissionProcessor;
        private readonly IPropertyService _propertyService;
        private readonly string _uploadPath;

        public LegacyController(ILCAVPSubmissionProcessor lcavpSubmissionProcessor, IConfiguration configuration, IPropertyService propertyService)
        {
            _lcavpSubmissionProcessor = lcavpSubmissionProcessor;
            _uploadPath = configuration.GetValue<string>("LCAVP:UploadPath");
            _propertyService = propertyService;
        }


        [HttpGet("{fileName}")]
        public Result DumbProcessTest(string fileName)
        {
            string filePath = Path.Combine(_uploadPath, fileName);
            _lcavpSubmissionProcessor.Process(filePath, "me@foo.com");
            return new Result();
        }

        [HttpGet("VerifyPersistedAlgorithmProperties")]
        public List<string> VerifyPersistedAlgorithmProperties()
        {
            return _propertyService.VerifyAlgorithmModels();
        }

        [HttpGet("AlgorithmModelTree/{algorithmClassName}")]
        public List<string> GetAlgorithmModelTree(string algorithmClassName)
        {
            return _propertyService.GetAlgorithmPropertyTree(algorithmClassName);
        }
    }
}