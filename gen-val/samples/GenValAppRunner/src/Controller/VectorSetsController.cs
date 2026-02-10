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
     private readonly IVectorSetService _vectorSetService;

     public VectorSetsController(
     IVectorSetService vectorSetService)
     {
        _vectorSetService = vectorSetService;
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

           var response = await _vectorSetService.ValidateAsync(request);

           return Ok(response);

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
      
           var response = await _vectorSetService.GenerateAsync(registration);

           return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
     }
    }
}
