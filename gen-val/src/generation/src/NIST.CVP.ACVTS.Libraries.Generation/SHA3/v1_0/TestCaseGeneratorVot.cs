using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
{
    public class TestCaseGeneratorVot : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 512;
        private ShuffleQueue<int> _outputSizes;

        public TestCaseGeneratorVot(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var outputAllowed = group.OutputLength.GetDeepCopy();
            var outputMinMax = outputAllowed.GetDomainMinMax();
            var outputLengths = new List<int>
            {
                outputMinMax.Minimum,
                outputMinMax.Maximum
            };

            // Keep pulling output lengths until we have enough
            do
            {
                outputLengths.AddRange(outputAllowed.GetRandomValues(x => x % (group.BitOrientedOutput ? 1 : 8) == 0, NumberOfTestCasesToGenerate));
            } while (outputLengths.Count < NumberOfTestCasesToGenerate);

            // Shuffle outputs
            _outputSizes = new ShuffleQueue<int>(outputLengths, NumberOfTestCasesToGenerate);

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var outputLength = _outputSizes.Pop();
            var param = new ShaParameters
            {
                HashFunction = group.CommonHashFunction,
                MessageLength = group.CommonHashFunction.OutputLen,
                OutputLength = outputLength
            };

            try
            {
                var oracleResult = await _oracle.GetSha3CaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = new BitString(oracleResult.Digest.ToLittleEndianHex()),
                    DigestLength = outputLength
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



