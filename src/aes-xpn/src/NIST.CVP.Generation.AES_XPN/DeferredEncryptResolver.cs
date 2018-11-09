using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_XPN
{
    public class DeferredEncryptResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>
    {
        private readonly IOracle _oracle;

        public DeferredEncryptResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<AeadResult> CompleteDeferredCryptoAsync(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iv = serverTestCase.IV.GetDeepCopy();
            if (testGroup.IvGeneration.Equals("internal", StringComparison.OrdinalIgnoreCase))
            {
                iv = iutTestCase.IV.GetDeepCopy();
            }

            var salt = serverTestCase.Salt;
            if (testGroup.SaltGen.Equals("internal", StringComparison.OrdinalIgnoreCase))
            {
                salt = iutTestCase.Salt.GetDeepCopy();
            }

            var ivXorSalt = salt.XOR(iv);

            var param = new AeadParameters
            {
                TagLength = testGroup.TagLength
            };

            var fullParam = new AeadResult
            {
                Iv = ivXorSalt,
                Key = serverTestCase.Key,
                PlainText = serverTestCase.PlainText,
                Aad = serverTestCase.AAD
            };

            return await _oracle.CompleteDeferredAesXpnCaseAsync(param, fullParam);
        }
    }
}