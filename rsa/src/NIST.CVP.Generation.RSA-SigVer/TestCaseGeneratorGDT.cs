using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseGeneratorGDT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly SignerBase _signer;

        private int _numCases = 6;

        public int NumberOfTestCasesToGenerate { get { return _numCases; } }

        public TestCaseGeneratorGDT(IRandom800_90 random800_90, SignerBase signer)
        {
            _random800_90 = random800_90;
            _signer = signer;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var shuffledReasons = group.Covered.OrderBy(a => Guid.NewGuid()).ToList();
            var reason = shuffledReasons[0];
            group.Covered.Remove(reason);

            var testCase = new TestCase
            {
                Message = _random800_90.GetRandomBitString(group.Modulo / 2),
                Reason = reason,
                FailureTest = (reason != FailureReasons.NONE)
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            SignatureResult sigResult = null;
            try
            {
                _signer.SetHashFunction(group.HashAlg);

                if(group.Mode == SigGenModes.PSS)
                {
                    testCase.Salt = _random800_90.GetRandomBitString(group.SaltLen * 8);
                    _signer.AddEntropy(testCase.Salt);
                }

                sigResult = _signer.SignWithErrors(group.Modulo, testCase.Message, group.Key, testCase.Reason);
                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating signature with intentional errors: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating signature with intentional errors: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Warn($"Exception generating signature with intentional errors: {sigResult.ErrorMessage}, {ex.Source}");
                return new TestCaseGenerateResponse($"Exception generating signature with intentional errors: {sigResult.ErrorMessage}");
            }

            testCase.Signature = sigResult.Signature;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
