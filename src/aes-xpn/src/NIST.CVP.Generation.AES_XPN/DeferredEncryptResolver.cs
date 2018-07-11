using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using System;

namespace NIST.CVP.Generation.AES_XPN
{
    public class DeferredEncryptResolver : IDeferredTestCaseResolver<TestGroup, TestCase, AeadResult>
    {
        private readonly IOracle _oracle;

        public DeferredEncryptResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public AeadResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iv = serverTestCase.IV.GetDeepCopy();
            if (testGroup.IVGeneration.Equals("internal", StringComparison.OrdinalIgnoreCase))
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

            return _oracle.CompleteDeferredAesXpnCase(param, fullParam);
        }
    }
}