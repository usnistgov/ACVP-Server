using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random;
        private readonly IDsaEccFactory _eccDsaFactory;
        private IDsaEcc _eccDsa;
        private readonly IEccCurveFactory _curveFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IRandom800_90 rand, IDsaEccFactory eccDsaFactory, IEccCurveFactory curveFactory)
        {
            _random = rand;
            _eccDsaFactory = eccDsaFactory;
            _curveFactory = curveFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            // For component test, get a post-hash message (just random value of hash output length)
            var testCase = new TestCase
            {
                Message = _random.GetRandomBitString(group.ComponentTest ? group.HashAlg.OutputLen : 1024)
            };

            if (isSample)
            {
                return Generate(group, testCase);
            }
            else
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            // Generate the signature
            EccSignatureResult sigResult = null;
            try
            {
                _eccDsa = _eccDsaFactory.GetInstance(group.HashAlg);
                var curve = _curveFactory.GetCurve(group.Curve);
                var domainParams = new EccDomainParameters(curve);
                sigResult = _eccDsa.Sign(domainParams, group.KeyPair, testCase.Message, group.ComponentTest);
                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating signature: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating signature: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Warn($"Exception generating signature: {sigResult.ErrorMessage}, {ex.Message}");
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception generating signature: {sigResult.ErrorMessage}");
            }

            testCase.Signature = sigResult.Signature;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
