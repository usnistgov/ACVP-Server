using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NIST.CVP.Pools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.PoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrleansPoolLogController : Controller
    {
        // TODO fix the way this is being done... relying on static yuckyness from Program.cs

        [HttpGet]
        public IEnumerable<PoolOrleansJob> GetLog()
        {
            return Program.PoolOrleansJobLog;
        }

        [HttpPost]
        public void Save()
        {
            System.IO.File.WriteAllText(
                Program.OrleansPoolLogLocation,
                JsonConvert.SerializeObject(
                    Program.PoolOrleansJobLog,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                )
            );
        }
    }
}
