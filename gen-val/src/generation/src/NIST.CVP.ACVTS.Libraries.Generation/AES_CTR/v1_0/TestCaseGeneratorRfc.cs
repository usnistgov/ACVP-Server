using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestCaseGeneratorRfc : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly List<int> _chosenPayloadLens = new List<int>();

        public TestCaseGeneratorRfc(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 25;

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            var blockSize = 128;

            // if the registration allows for partial blocks
            if (group.PayloadLength.ContainsValueOtherThan(blockSize))
            {
                var nonBlockValues = group.PayloadLength.GetRandomValues(10).ToList();
                _chosenPayloadLens.AddRangeIfNotNullOrEmpty(nonBlockValues);
            }

            // full blocks
            var i = 1;
            while (_chosenPayloadLens.Count < NumberOfTestCasesToGenerate)
            {
                _chosenPayloadLens.Add(i++ * blockSize);
            }

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            var payloadLen = _chosenPayloadLens[caseNo];
            var counterParameters = new CounterParameters<AesParameters>()
            {
                Incremental = true,
                Overflow = false,
                Parameters = new AesParameters()
                {
                    Mode = BlockCipherModesOfOperation.Ctr,
                    Direction = group.Direction,
                    DataLength = payloadLen,
                    KeyLength = group.KeyLength
                }
            };

            // if internal IV, then deferred, but only if not sample
            if (group.Direction.Equals("encrypt", StringComparison.OrdinalIgnoreCase) &&
                group.IvGenMode == IvGenModes.Internal && !isSample)
            {
                // we're going to ignore the IV that's generated, since the IUT will be providing it for deferred internal cases
                var deferredResult = await _oracle.GetDeferredAesCounterCaseAsync(counterParameters);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
                {
                    Deferred = true,
                    Key = deferredResult.Key,
                    PayloadLength = payloadLen,
                    PlainText = deferredResult.PlainText,
                });
            }

            // otherwise we can generate both encrypt and decrypt cases
            var result = await _oracle.GetAesCounterRfcCaseAsync(counterParameters);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase()
            {
                Deferred = group.Direction.Equals("encrypt", StringComparison.OrdinalIgnoreCase) && group.IvGenMode == IvGenModes.Internal,
                Key = result.Key,
                IV = result.Iv,
                PayloadLength = payloadLen,
                PlainText = result.PlainText,
                CipherText = result.CipherText,
            });
        }
    }
}
