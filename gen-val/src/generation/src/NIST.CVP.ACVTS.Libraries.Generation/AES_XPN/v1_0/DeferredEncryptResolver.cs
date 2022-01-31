using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0
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
            BitString iv = null;
            if (testGroup.IvGeneration.Equals("internal", StringComparison.OrdinalIgnoreCase))
            {
                iv = iutTestCase.IV?.GetDeepCopy();
            }
            else
            {
                iv = serverTestCase.IV?.GetDeepCopy();
            }

            BitString salt = null;
            if (testGroup.SaltGen.Equals("internal", StringComparison.OrdinalIgnoreCase))
            {
                salt = iutTestCase.Salt.GetDeepCopy();
            }
            else
            {
                salt = serverTestCase.Salt.GetDeepCopy();
            }

            var param = new AeadParameters
            {
                TagLength = testGroup.TagLength
            };

            var fullParam = new AeadResult
            {
                Iv = iv,
                Salt = salt,
                Key = serverTestCase.Key,
                PlainText = serverTestCase.PlainText,
                Aad = serverTestCase.AAD
            };

            return await _oracle.CompleteDeferredAesXpnCaseAsync(param, fullParam);
        }
    }
}
