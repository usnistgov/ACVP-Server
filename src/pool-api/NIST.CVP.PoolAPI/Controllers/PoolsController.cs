using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Pools;
using System.Linq;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.PoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolsController : ControllerBase
    {
        //private readonly PoolManager _poolManager = new PoolManager(@"D:\ACVP\gen-val\src\pool-api\NIST.CVP.PoolAPI\Pools\testConfig.json");
        private readonly PoolManager _poolManager = new PoolManager(@"C:\Users\ctc\Documents\ACVP\gen-val\src\pool-api\NIST.CVP.PoolAPI\Pools\testConfig.json");
        
        [HttpGet]
        public ActionResult<PoolResult<AesResult>> Get()
        {
            // TODO this is not a good way of doing this
            var param = new AesParameters
            {
                KeyLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                Direction = "encrypt",
                DataLength = 128
            };
            return _poolManager.GetAesResultFromPool(param);
            //return _poolManager._aesPools.First().FilePath;
        }
    }
}
