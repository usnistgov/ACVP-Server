using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.RSA.Fips186_5.KeyGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.IntegrationTests.Fips186_5
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "KeyGen";
        public override string Revision { get; set; } = "FIPS186-5";

        public override AlgoMode AlgoMode => AlgoMode.RSA_KeyGen_Fips186_5;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.testPassed != null)
            {
                testCase.testPassed = !(bool) testCase.testPassed;
            }

            // If TC has a cipherText, change it
            if (testCase.p != null)
            {
                BitString bs = new BitString(testCase.p.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.p = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.q != null)
            {
                BitString bs = new BitString(testCase.q.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.q = bs.ToHex();
            }
        }
        
        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new []
            {
                new Capability
                {
                    Modulo = 2048,
                    HashAlgs = new[] { "sha2-224" },
                    PrimeTests = new[] { PrimeTestModes.TwoPow100ErrorBound },
                    PMod8 = 1,
                    QMod8 = 7
                }
            };

            var algSpecs = new []
            {
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProvablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes,
                    Capabilities = caps
                } 
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                InfoGeneratedByServer = false,
                IsSample = true,
                PubExpMode = PublicExponentModes.Fixed,
                FixedPubExp = new BitString("010001"),
                KeyFormat = PrivateKeyModes.Crt,
                AlgSpecs = algSpecs
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var algSpecs = new []
            {
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProvablePrimes,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 2048,
                            HashAlgs = new [] { "sha2-224" },
                            PrimeTests = new [] {PrimeTestModes.TwoPowSecurityStrengthErrorBound},
                            PMod8 = 1,
                            QMod8 = 1
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimes,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 3072,
                            HashAlgs = new [] { "sha2-256" },
                            PrimeTests = new [] {PrimeTestModes.TwoPow100ErrorBound},
                            PMod8 = 5,
                            QMod8 = 3
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 4096,
                            HashAlgs = new [] { "sha2-512" },
                            PrimeTests = new [] {PrimeTestModes.TwoPowSecurityStrengthErrorBound},
                            PMod8 = 7,
                            QMod8 = 0
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 2048,
                            HashAlgs = new [] { "sha2-224" },
                            PrimeTests = new [] {PrimeTestModes.TwoPowSecurityStrengthErrorBound},
                            PMod8 = 0,
                            QMod8 = 1
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 4096,
                            HashAlgs = new [] { "sha2-512" },
                            PrimeTests = new [] {PrimeTestModes.TwoPowSecurityStrengthErrorBound},
                            PMod8 = 3,
                            QMod8 = 5
                        }
                    }
                } 
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                InfoGeneratedByServer = true,
                IsSample = true,
                PubExpMode = PublicExponentModes.Random,
                AlgSpecs = algSpecs,
                KeyFormat = PrivateKeyModes.Crt
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}