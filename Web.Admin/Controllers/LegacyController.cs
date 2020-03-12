using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
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

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public ActionResult<string> Upload()
        {
            try
            {
                var uploadedFile = Request.Form.Files[0];

                if (uploadedFile.Length > 0)
                {
                    //Get the file name
                    string fileName = ContentDispositionHeaderValue.Parse(uploadedFile.ContentDisposition).FileName.Trim('"');

                    //Combine with the upload root to give us the full path the file will be saved as 
                    string destinationPath = Path.Combine(_uploadPath, fileName);

                    //Save the file to that location
                    using var stream = new FileStream(destinationPath, FileMode.Create);
                    uploadedFile.CopyTo(stream);
                    stream.Close();

                    //Call LCAVP
                    _lcavpSubmissionProcessor.Process(destinationPath);
                }
                return "Guess it worked?";
            }
            catch (Exception ex)
            {
                var doingThisToAvoidTheWarningThatIsTreatedLikeAnError = ex;
                return "Upload failed";
            }
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