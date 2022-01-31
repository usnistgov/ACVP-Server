using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class DeferredRfcResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, AesResult>
    {
        private readonly IOracle _oracle;

        public DeferredRfcResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<AesResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var result = await _oracle.CompleteDeferredAesCounterRfcCaseAsync(new AesWithPayloadParameters()
            {
                Direction = BlockCipherDirections.Encrypt,
                Iv = iutTestCase.IV,
                Key = serverTestCase.Key,
                Mode = BlockCipherModesOfOperation.Ctr,
                Payload = serverTestCase.PlainText.GetMostSignificantBits(serverTestCase.PayloadLength)
            });

            return result;
        }
    }
}
