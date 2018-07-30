using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestCaseGeneratorMct : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly int _stringBitLength = 160;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var param = new ParallelHashParameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2, group.XOF),
                MessageLength = group.DigestSize,
                OutLens = group.OutputLength.GetDeepCopy()
            };

            MctResult<HashResultParallelHash> oracleResult = null;
            try
            {
                oracleResult = _oracle.GetParallelHashMctCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Message = oracleResult.Results[0].Message,
                Digest = oracleResult.Results[0].Digest,
                ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponseWithCustomization { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
