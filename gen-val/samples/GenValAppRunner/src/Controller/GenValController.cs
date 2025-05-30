using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;

namespace GenValApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GenValController : ControllerBase
    {
        public GenValController()
        {}

        [HttpGet("algorithms")]
        public IActionResult GetSupportedAlgorithms()
        {
            try
            {
                var algoModes = AutofacConfig.GetSupportedAlgoModeInfos();

                return Ok(algoModes);
            }
            catch (Exception ex)
            {
                  return StatusCode(500, new
                     {
                          message = "Failed to retrieve supported algorithms.",
                          error = ex.Message
                     });
            }
        }
    }
}