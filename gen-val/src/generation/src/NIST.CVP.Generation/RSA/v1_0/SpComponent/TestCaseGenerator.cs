﻿using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.RSA.v1_0.SpComponent
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 30;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var param = new RsaSignaturePrimitiveParameters
            {
                KeyFormat = group.KeyFormat,
                Modulo = group.Modulo
            };

            try
            {
                var result = await _oracle.GetRsaSignaturePrimitiveAsync(param);

                var testCase = new TestCase
                {
                    Signature = result.Signature?.PadToModulusMsb(group.Modulo),
                    Key = result.Key,
                    Message = result.Message,
                    TestPassed = result.ShouldPass     // Failure test if m > N, meaning it can't be signed
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
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