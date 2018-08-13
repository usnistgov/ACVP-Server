using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.KeyWrap;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly KeyWrapFactory _keyWrapFactory = new KeyWrapFactory();

        private KeyWrapResult GetKeyWrapCase(KeyWrapParameters param)
        {
            var keyWrap = _keyWrapFactory.GetKeyWrapInstance(param.KeyWrapType);

            var key = _rand.GetRandomBitString(param.KeyLength);
            var payload = _rand.GetRandomBitString(param.DataLength);

            var result = new KeyWrapResult()
            {
                Key = key,
                Plaintext = payload,
                Ciphertext = keyWrap.Encrypt(key, payload, param.WithInverseCipher).Result,
                TestPassed = true
            };

            if (param.CouldFail)
            {
                // Should Fail at certain ratio, 20%
                var upperBound = (int)(1.0 / KEYWRAP_FAIL_RATIO);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result.Ciphertext = _rand.GetDifferentBitStringOfSameSize(result.Ciphertext);
                    result.TestPassed = false;
                }
            }

            return result;
        }


        public async Task<KeyWrapResult> GetKeyWrapCaseAsync(KeyWrapParameters param)
        {
            return await _taskFactory.StartNew(() => GetKeyWrapCase(param));
        }
    }
}
