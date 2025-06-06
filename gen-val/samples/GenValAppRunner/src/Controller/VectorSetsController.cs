using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
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


[ApiController]
[Route("api/v1/vectorsets")]
public class VectorSetsController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public VectorSetsController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] Registration registration)
    {
     try{
          var registrationString = JsonConvert.SerializeObject(registration);
          var registrationJson = JObject.Parse(registrationString);


           var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(registration.Algorithm, "", registration.Revision);

            // Dynamically configure container for algorithm
            AutofacConfig.IoCConfiguration(_serviceProvider, algoMode);
            using var scope = AutofacConfig.GetContainer().BeginLifetimeScope();

            var generator = scope.Resolve<IGenerator>();
            var response = await generator.GenerateAsync(new GenerateRequest(registrationString));

            return Ok(new
            {
                response.StatusCode,
                response.ErrorMessage,
                response.ResultProjection 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
