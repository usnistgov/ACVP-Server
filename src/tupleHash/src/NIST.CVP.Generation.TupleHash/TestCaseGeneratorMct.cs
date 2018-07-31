using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseGeneratorMct : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private const int INITIAL_MESSAGE_LEN = 288;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var param = new TupleHashParameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2, group.XOF),
                MessageLength = INITIAL_MESSAGE_LEN,
                OutLens = group.OutputLength.GetDeepCopy()
            };

            MctResult<TupleHashResult> oracleResult = null;
            try
            {
                oracleResult = _oracle.GetTupleHashMctCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Tuple = oracleResult.Results[0].Tuple,
                Digest = oracleResult.Results[0].Digest,
                ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponse { Tuple = element.Tuple, Digest = element.Digest, Customization = element.Customization })
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
