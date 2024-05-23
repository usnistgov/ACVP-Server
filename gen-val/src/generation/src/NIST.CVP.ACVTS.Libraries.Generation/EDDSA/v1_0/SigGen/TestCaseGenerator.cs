using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _contextLengths;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var min = group.ContextLength.GetDomainMinMax().Minimum == 0 ? 1 : group.ContextLength.GetDomainMinMax().Minimum;
            var max = group.ContextLength.GetDomainMinMax().Maximum == 0 ? 1 : group.ContextLength.GetDomainMinMax().Maximum;
            
            var lengths = group.ContextLength.GetRandomValues(min, max, NumberOfTestCasesToGenerate - 2).ToList();
            lengths.Add(min);
            lengths.Add(max);
            
            _contextLengths = new ShuffleQueue<int>(lengths, NumberOfTestCasesToGenerate);
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new EddsaSignatureParameters
            {
                Curve = group.Curve,
                PreHash = group.PreHash,
                Key = group.KeyPair,
                ContextLength = _contextLengths.Pop()
            };
            
            try
            {
                TestCase testCase = null;
                EddsaSignatureResult result = null;
                if (isSample)
                {
                    result = await _oracle.GetEddsaSignatureAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message,
                        Context = result.Context,
                        ContextLength = result.ContextLength,
                        Signature = result.Signature
                    };
                }
                else
                {
                    result = await _oracle.GetDeferredEddsaSignatureAsync(param);
                    testCase = new TestCase
                    {
                        Message = result.Message,
                        Context = result.Context,
                        ContextLength = result.ContextLength
                    };
                }
                
                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
