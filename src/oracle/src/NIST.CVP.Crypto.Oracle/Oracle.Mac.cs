using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly CmacFactory _cmacFactory = new CmacFactory();
        private readonly HmacFactory _hmacFactory = new HmacFactory(new ShaFactory());

        public MacResult GetCmacCase(CmacParameters param)
        {
            var cmac = _cmacFactory.GetCmacInstance(param.CmacType);

            BitString key = null;

            if (param.KeyingOption is default(int))
            {
                key = _rand.GetRandomBitString(param.KeyLength);
            }
            else
            {
                key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            }
            
            var msg = _rand.GetRandomBitString(param.PayloadLength);

            var mac = cmac.Generate(key, msg, param.MacLength);
            var result = new MacResult()
            {
                Key = key,
                Message = msg,
                Tag = mac.Mac,
                TestPassed = true
            };

            if (param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / CMAC_FAIL_RATIO);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result.Tag = _rand.GetDifferentBitStringOfSameSize(result.Tag);
                    result.TestPassed = false;
                }
            }

            return result;
        }

        public MacResult GetHmacCase(HmacParameters param)
        {
            var hmac = _hmacFactory.GetHmacInstance(new HashFunction(param.ShaMode, param.ShaDigestSize));

            var key = _rand.GetRandomBitString(param.KeyLength);
            var msg = _rand.GetRandomBitString(param.MessageLength);

            var mac = hmac.Generate(key, msg, param.MacLength);
            var result = new MacResult()
            {
                Key = key,
                Message = msg,
                Tag = mac.Mac
            };

            return result;
        }

        public async Task<MacResult> GetCmacCaseAsync(CmacParameters param)
        {
            return await _taskFactory.StartNew(() => GetCmacCase(param));
        }

        public async Task<MacResult> GetHmacCaseAsync(HmacParameters param)
        {
            return await _taskFactory.StartNew(() => GetHmacCase(param));
        }
    }
}
