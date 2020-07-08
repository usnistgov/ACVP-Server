using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "KeyGen";

        public override AlgoMode AlgoMode => AlgoMode.RSA_KeyGen_v1_0;

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
                    PrimeTests = new[] { PrimeTestFips186_4Modes.TblC3 }    
                }
            };

            var algSpecs = new []
            {
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B32,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B33,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B34,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B35,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B36,
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
                    RandPQ = PrimeGenFips186_4Modes.B32,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 2048,
                            HashAlgs = new [] { "sha2-224" }
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B33,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 3072,
                            PrimeTests = new [] {PrimeTestFips186_4Modes.TblC3}    
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B34,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 4096,
                            HashAlgs = new [] { "sha2-512" }
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B35,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 2048,
                            HashAlgs = new [] { "sha2-224" },
                            PrimeTests = new [] {PrimeTestFips186_4Modes.TblC2}    
                        }
                    }
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B36,
                    Capabilities = new []
                    {
                        new Capability
                        {
                            Modulo = 4096,
                            PrimeTests = new [] {PrimeTestFips186_4Modes.TblC3}    
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
