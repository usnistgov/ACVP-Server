using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA_SigGen.IntegrationTests.Fips186_5
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "sigGen";
        public override string Revision { get; set; } = "FIPS186-5";

        public override AlgoMode AlgoMode => AlgoMode.RSA_SigGen_Fips186_5;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a signature, change it
            if (testCase.signature != null)
            {
                BitString bs = new BitString(testCase.signature.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.signature = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var hashPairsPss = new[]
            {
                new HashPair
                {
                    HashAlg = "SHA2-256",
                    SaltLen = 8
                },
                new HashPair
                {
                    HashAlg = "SHA3-512",
                    SaltLen = 62
                }
            };
            
            var hashPairsPkcs1v15 = new[]
            {
                new HashPair
                {
                    HashAlg = "SHA2-256"
                },
                new HashPair
                {
                    HashAlg = "SHA3-512"
                }
            };
            
            var modCapPss = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    MaskFunction = new [] {PssMaskTypes.MGF1},
                    HashPairs = hashPairsPss
                }
            };

            var modCapPkcs1v15 = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    HashPairs = hashPairsPkcs1v15
                }
            };
            
            var algSpecs = new AlgSpecs[2];
            
            algSpecs[0] = new AlgSpecs
            {
                SigType = SignatureSchemes.Pss,
                ModuloCapabilities = modCapPss
            };
            
            algSpecs[1] = new AlgSpecs
            {
                SigType = SignatureSchemes.Pkcs1v15,
                ModuloCapabilities = modCapPkcs1v15
            };
            
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = algSpecs,
                IsSample = true,
                Conformances = new[] { "SP800-106" }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var hashPairsPkcs = new[]
            {
                new HashPair
                {
                    HashAlg = "SHA2-256"
                },
                new HashPair
                {
                    HashAlg = "SHA3-512"
                }
            };

            var hashPairsPss = new[]
            {
                new HashPair
                {
                    HashAlg = "SHAKE-128",
                    SaltLen = 32
                },
                new HashPair
                {
                    HashAlg = "SHAKE-256",
                    SaltLen = 64
                }
                ,
                new HashPair
                {
                    HashAlg = "SHA2-256",
                    SaltLen = 8
                }
            };
            
            var modCapPkcs = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    HashPairs = hashPairsPkcs
                },
                new CapSigType
                {
                    Modulo = 3072,
                    HashPairs = hashPairsPkcs
                },
                 new CapSigType
                 {
                     Modulo = 4096,
                     HashPairs = hashPairsPkcs
                 }
            };

            var modCapPss = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    MaskFunction = new [] { PssMaskTypes.MGF1 },
                    HashPairs = hashPairsPss
                },
                new CapSigType
                {
                    Modulo = 3072,
                    MaskFunction = new [] { PssMaskTypes.SHAKE128 },
                    HashPairs = hashPairsPss
                },
                new CapSigType
                {
                    Modulo = 4096,
                    MaskFunction = new [] { PssMaskTypes.SHAKE256 },
                    HashPairs = hashPairsPss
                }
            };

            var algSpecs = new[]
            {
                new AlgSpecs
                {
                    SigType = SignatureSchemes.Pkcs1v15, 
                    ModuloCapabilities = modCapPkcs
                },
                new AlgSpecs
                {
                    SigType = SignatureSchemes.Pss, 
                    ModuloCapabilities = modCapPss
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = algSpecs,
                IsSample = true,
                Conformances = new[] { "SP800-106" }
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
