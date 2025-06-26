using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using GenValAppRunner.DTO;

namespace GenValApp.Controllers
{
    [ApiController]
    [Route("api/v1/vectorsets")]
    public class VectorSetsController : ControllerBase
    {
     private readonly IGeneratorResolver _generatorResolver;
     private readonly IValidationResolver _validationResolver;


     public VectorSetsController(IGeneratorResolver generatorResolver,
     IValidationResolver validationResolver)
     {
        _generatorResolver = generatorResolver;
        _validationResolver = validationResolver;
     }

    [HttpPost("validate")]
    public async Task<ActionResult<ValidationResponse>> Validate(ValidationRequest request)
    {
        try
        {  
           if (!ModelState.IsValid)
           {
             return BadRequest(ModelState);
           }
           var answerString = JsonConvert.SerializeObject(request.Answer);
           var expectedString = JsonConvert.SerializeObject(request.Expected);

           var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(request.Answer.Algorithm, "", request.Answer.Revision);
        
           var (validator, scope) = _validationResolver.Resolve(algoMode);
           using (scope) // ensure scope is disposed
           {
            var response = await validator.ValidateAsync(new ValidateRequest(answerString,expectedString, true));

            return Ok(new ValidationResponse
            {
               StatusCode = response.StatusCode,
               ErrorMessage = response.ErrorMessage,
               Result = JsonConvert.DeserializeObject<VectorSetValidationResults>(response.ValidationResult)
            });
           }

        }catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("generate")]
    public async Task<ActionResult<VectorSetResponse>> Generate([FromBody] Registration registration)
    {
     try{
          if (!ModelState.IsValid)
           {
             return BadRequest(ModelState);
           }
           var registrationString = JsonConvert.SerializeObject(registration);

           var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(registration.Algorithm, "", registration.Revision);
    
           var (generator, scope) = _generatorResolver.Resolve(algoMode);
           using (scope) // ensure scope is disposed
           {
           var response = await generator.GenerateAsync(new GenerateRequest(registrationString));

            return Ok(new VectorSetResponse
                {
                     StatusCode = response.StatusCode,
                     ErrorMessage = response.ErrorMessage,
                     Result = JsonConvert.DeserializeObject<TestVectorSet>(response.InternalProjection)
       
                });
           }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
     }
    }
}
