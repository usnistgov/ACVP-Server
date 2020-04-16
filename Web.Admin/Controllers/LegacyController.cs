using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using ACVPCore.Services;
using LCAVPCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NIST.CVP.Email;

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegacyController : ControllerBase
    {
        private readonly ILCAVPSubmissionProcessor _lcavpSubmissionProcessor;
        private readonly IPropertyService _propertyService;
        private readonly IMailer _mailer;
        private readonly string _uploadPath;

        public LegacyController(ILCAVPSubmissionProcessor lcavpSubmissionProcessor, IConfiguration configuration, IPropertyService propertyService, IMailer mailer)
        {
            _lcavpSubmissionProcessor = lcavpSubmissionProcessor;
            _uploadPath = configuration.GetValue<string>("LCAVP:UploadPath");
            _propertyService = propertyService;
            _mailer = mailer;
        }

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public ActionResult<SubmissionProcessingResult> Upload()
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
                    var result = _lcavpSubmissionProcessor.Process(destinationPath);

                    if (result.Success)
                    {
                        //Do email
                        string subject = $"CAVP submission {result.SubmissionID} processed";
                        string body = $"CAVP submission {result.SubmissionID} has been processed. " +  (result.SubmissionType == SubmissionType.New ? $"Validation number C{result.ValidationNumber} has been issued" : "All requested modifications have been made to the referenced validations") + ". Please direct any further questions to cavpval@nist.gov.";

                        _mailer.Send(subject, body, result.LabPOCEmail);
                    }

                    //Return the result so the UI can display something
                    return result;
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                var doingThisToAvoidTheWarningThatIsTreatedLikeAnError = ex;
                return new BadRequestResult();
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