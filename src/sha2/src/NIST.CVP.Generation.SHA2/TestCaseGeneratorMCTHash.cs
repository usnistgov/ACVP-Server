using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NLog;
using HashResult = NIST.CVP.Common.Oracle.ResultTypes.HashResult;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorMCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMCTHash(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var param = new HashParameters
            {
                HashFunction = new HashFunction(group.Function, group.DigestSize),
                MessageLength = SHAEnumHelpers.DigestSizeToInt(group.DigestSize)
            };

            MctResult<HashResult> oracleResult = null;
            try
            {
                oracleResult = _oracle.GetShaMctCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Message = oracleResult.Results[0].Message,
                Digest = oracleResult.Results[0].Digest,
                ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponse { Message = element.Message, Digest = element.Digest })
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

