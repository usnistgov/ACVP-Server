using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly CmacFactory _cmacFactory = new CmacFactory();

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

        public MacResult GetHmacCase() => throw new NotImplementedException();
    }
}
