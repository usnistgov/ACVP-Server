using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            DrbgResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetDrbgCase(group.DrbgParameters);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            if (oracleResult.Status == DrbgStatus.Success)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(
                    new TestCase {
                        ReturnedBits = oracleResult.ReturnedBits
                    }
                );
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(oracleResult.Status.ToString());
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }
    }
}

