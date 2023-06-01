using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigVer;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA_SigVer.IntegrationTests.Fips186_5
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "sigVer";
        public override string Revision { get; set; } = "FIPS186-5";

        public override AlgoMode AlgoMode => AlgoMode.RSA_SigVer_Fips186_5;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a result, change it
            if (testCase.testPassed != null)
            {
                testCase.testPassed = !(bool)testCase.testPassed;
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var hashPairs = new HashPair[2];
            for (var i = 0; i < hashPairs.Length; i++)
            {
                hashPairs[i] = new HashPair
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i + 2],
                    SaltLen = i + 1
                };
            }

            var modCap = new CapSigType[1];
            modCap[0] = new CapSigType
            {
                Modulo = 2048,
                MaskFunction = new[] { PssMaskTypes.MGF1 },
                HashPairs = hashPairs
            };

            var algSpecs = new AlgSpecs[ParameterValidator.VALID_SIG_VER_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpecs
                {
                    SigType = ParameterValidator.VALID_SIG_VER_MODES[i],
                    ModuloCapabilities = modCap
                };
            }

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Capabilities = algSpecs,
                PubExpMode = PublicExponentModes.Random,
                KeyFormat = "standard"
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var hashPairsPkcs = new[]
            {
                new HashPair
                {
                    HashAlg = "SHA2-256",
                    SaltLen = 1
                },
                new HashPair
                {
                    HashAlg = "SHA2-256",
                    SaltLen = 2
                }
            };

            var hashPairsPss = new[]
            {
                new HashPair
                {
                    HashAlg = "SHAKE-128",
                    SaltLen = 1
                },
                new HashPair
                {
                    HashAlg = "SHAKE-256",
                    SaltLen = 64
                },
                new HashPair
                {
                    HashAlg = "SHA3-256",
                    SaltLen = 32
                }
            };
            
            var masks = EnumHelpers.GetEnumsWithoutDefault<PssMaskTypes>();

            var modCapPkcs = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    MaskFunction = new[] { masks[0 % masks.Count] },
                    HashPairs = hashPairsPkcs
                },
                new CapSigType
                {
                    Modulo = 3072,
                    MaskFunction = new[] { masks[1 % masks.Count] },
                    HashPairs = hashPairsPkcs
                },
                new CapSigType
                {
                    Modulo = 4096,
                    MaskFunction = new[] { masks[2 % masks.Count] },
                    HashPairs = hashPairsPkcs
                }
            };

            var modCapPss = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    MaskFunction = new[] { masks[0 % masks.Count] },
                    HashPairs = hashPairsPss
                },
                new CapSigType
                {
                    Modulo = 3072,
                    MaskFunction = new[] { masks[1 % masks.Count] },
                    HashPairs = hashPairsPss
                },
                new CapSigType
                {
                    Modulo = 4096,
                    MaskFunction = new[] { masks[2 % masks.Count] },
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
                IsSample = true,
                Capabilities = algSpecs,
                PubExpMode = PublicExponentModes.Random,
                KeyFormat = "crt"
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
