using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.BlockCipher_DF
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        private ShuffleQueue<int> _payloadLengths;
        private readonly IOracle _oracle;

        private static readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var payloadLength = group.PayloadLen.GetDeepCopy();
            payloadLength.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var min = payloadLength.GetDomainMinMax().Minimum;
            var max = payloadLength.GetDomainMinMax().Maximum;

            var lengths = payloadLength.GetValues(x => x != min && x != max, NumberOfTestCasesToGenerate - 2, true).ToList();
            lengths.Add(min);
            lengths.Add(max);

            _payloadLengths = new ShuffleQueue<int>(lengths, NumberOfTestCasesToGenerate);

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            var param = new BlockCipherDfParameters
            {
                OutputLength = group.OutputLen,
                DataLength = _payloadLengths.Pop(),
                KeyLength = group.KeyLength
            };

            try
            {
                var oracleResult = await _oracle.GetBlockCipherDfCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    InputString = oracleResult.PlainText,
                    RequestedBits = oracleResult.CipherText
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
    }
}
