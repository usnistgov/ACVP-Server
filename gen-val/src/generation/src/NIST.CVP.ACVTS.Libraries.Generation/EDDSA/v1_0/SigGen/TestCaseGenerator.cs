using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
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
        private bool _noContext = false;
        
        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            // context is not applicable when the ED-25519 curve is used with the pure signature type
            _noContext = group.Curve == Curve.Ed25519 && !group.PreHash;
            
            // if context is applicable, generate a list of context lengths to test
            if (!_noContext)
            {
                var min = group.ContextLength.GetDomainMinMax().Minimum;
                var max = group.ContextLength.GetDomainMinMax().Maximum;
            
                // We always add min and max, so get between min+1 and max-1
                var lengths = group.ContextLength.GetRandomValues(min+1, max-1, NumberOfTestCasesToGenerate - 2).ToList();
                lengths.Add(min);
                lengths.Add(max);
            
                _contextLengths = new ShuffleQueue<int>(lengths, NumberOfTestCasesToGenerate);           
            }
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            EddsaSignatureParameters param = new EddsaSignatureParameters
            {
                Curve = group.Curve,
                PreHash = group.PreHash,
                Key = group.KeyPair
            };
            
            // if context is applicable, add it into the passing parameters? 
            if (!_noContext)
            {
                param.ContextLength = _contextLengths.Pop();
            }
            
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
