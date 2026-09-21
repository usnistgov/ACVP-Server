using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class DeferredResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, AesXtsResult>
    {
        private readonly IOracle _oracle;

        public DeferredResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<AesXtsResult> CompleteDeferredCryptoAsync(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new AesXtsParameters
            {
                Direction = EnumHelpers.GetEnumDescriptionFromEnum(testGroup.Direction),
                DataUnitLength = serverTestCase.DataUnitLen
            };

            var fullParam = new AesXtsResult
            {
                Iv = iutTestCase.I,
                Key = serverTestCase.Key,
                PlainText = serverTestCase.PlainText?.GetMostSignificantBits(serverTestCase.PayloadLen),
                CipherText = serverTestCase.CipherText?.GetMostSignificantBits(serverTestCase.PayloadLen),
            };

            return await _oracle.CompleteDeferredAesXtsCaseAsync(param, fullParam);
        }
    }
}
