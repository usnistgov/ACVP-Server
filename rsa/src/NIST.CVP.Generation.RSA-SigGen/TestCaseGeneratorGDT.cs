using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseGeneratorGDT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly SignerBase _signer;
        private readonly PrimeGeneratorBase _primeGen;

        private int _numCases = 10;

        public int NumberOfTestCasesToGenerate { get { return _numCases; } }

        public TestCaseGeneratorGDT(IRandom800_90 random800_90, SignerBase signer, PrimeGeneratorBase primeGen = null)
        {
            _random800_90 = random800_90;
            _signer = signer;
            _primeGen = primeGen;

            if(_primeGen == null)
            {
                _primeGen = new RandomProbablePrimeGenerator(Math.Entropy.EntropyProviderTypes.Random);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _numCases = 3;
            }

            // Only make one key per group
            if(group.Key == null && isSample)
            {
                BigInteger E = 3;
                PrimeGeneratorResult primeResult = null;

                do
                {
                    //ThisLogger.Debug($"Computing key for {group.Modulo}");
                    E = RSAEnumHelpers.GetEValue();
                    primeResult = _primeGen.GeneratePrimes(group.Modulo, E, RSAEnumHelpers.GetSeed(group.Modulo));
                } while (!primeResult.Success);

                group.Key = new KeyPair(primeResult.P, primeResult.Q, E);
            }

            var testCase = new TestCase
            {
                Message = _random800_90.GetRandomBitString(group.Modulo / 2),
                IsSample = isSample
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            if (testCase.IsSample)
            {
                SignatureResult sigResult = null;
                try
                {
                    _signer.SetHashFunction(group.HashAlg);

                    if (group.Mode == SigGenModes.PSS)
                    {
                        testCase.Salt = _random800_90.GetRandomBitString(group.SaltLen * 8);
                        _signer.AddEntropy(testCase.Salt);
                    }

                    sigResult = _signer.Sign(group.Modulo, testCase.Message, group.Key);
                    if (!sigResult.Success)
                    {
                        ThisLogger.Warn($"Error generating sample signature: {sigResult.ErrorMessage}");
                        return new TestCaseGenerateResponse($"Error generating sample signature: {sigResult.ErrorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    ThisLogger.Error($"Exception generating sample signature: {sigResult.ErrorMessage}; {ex.StackTrace}");
                    return new TestCaseGenerateResponse($"Exception generating sample signature: {sigResult.ErrorMessage}");
                }

                testCase.Signature = sigResult.Signature;
            }

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
