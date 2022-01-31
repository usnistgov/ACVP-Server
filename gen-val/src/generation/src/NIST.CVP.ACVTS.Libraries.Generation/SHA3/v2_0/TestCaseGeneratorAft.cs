using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; set; }
        private ShuffleQueue<int> _messageLengths { get; set; }

        private readonly IOracle _oracle;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        // Some arbitrary numbers are used here (100 and 120). These are to just make the test count similar to revision 1.0/CAVS
        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var messageLengths = group.MessageLengths.GetDeepCopy();
            messageLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var minMax = messageLengths.GetDomainMinMax();

            var lengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            var rate = 1600 - (group.HashFunction.OutputLen * 2);

            // Pull all valid values up to 'rate'
            lengths.AddRangeIfNotNullOrEmpty(messageLengths.GetValues(x => x <= rate, rate, true));

            // Pull 100 values greater than 'rate'
            lengths.AddRangeIfNotNullOrEmpty(messageLengths.GetValues(x => x > rate, 100, true));

            // Remove duplicated values
            lengths = lengths.Distinct().ToList();

            // Make sure at least 120 tests are run
            NumberOfTestCasesToGenerate = System.Math.Max(120, lengths.Count);
            _messageLengths = new ShuffleQueue<int>(lengths, NumberOfTestCasesToGenerate);

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            var param = new ShaParameters
            {
                HashFunction = group.HashFunction,
                MessageLength = _messageLengths.Pop()
            };

            try
            {
                var oracleResult = await _oracle.GetSha3CaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
