using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;
using System.Linq;

namespace NIST.CVP.PoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolsController : ControllerBase
    {
        private readonly PoolManager _poolManager = new PoolManager(@"D:\ACVP\gen-val\src\pools\testFiles\testConfig.json");

        [HttpGet]
        public ActionResult<PoolResult<AesResult>> Get()
        {
            return _poolManager._aesPools.First().GetNext();
        }
    }
}
