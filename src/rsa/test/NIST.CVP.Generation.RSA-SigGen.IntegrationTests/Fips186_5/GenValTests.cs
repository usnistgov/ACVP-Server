using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.RSA.Fips186_5.SigGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_SigGen.IntegrationTests.Fips186_5
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "SigGen";
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
            var hashPairs = new []
            {
                new HashPair
                {
                    HashAlg = "sha2-256",
                    SaltLen = 8
                }, 
                new HashPair
                {
                    HashAlg = "sha2-512",
                    SaltLen = 62
                } 
            };

            var modCap = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    MaskFunction = new [] {PssMaskTypes.MGF1},
                    HashPairs = hashPairs
                }
            };
            
            var algSpecs = new AlgSpecs[ParameterValidator.VALID_SIG_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpecs
                {
                    SigType = ParameterValidator.VALID_SIG_GEN_MODES[i],
                    ModuloCapabilities = modCap
                };
            }

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
            var hashPairs = new []
            {
                new HashPair
                {
                    HashAlg = "sha2-256",
                    SaltLen = 8
                }, 
                new HashPair
                {
                    HashAlg = "sha2-512",
                    SaltLen = 62
                } 
            };

            var modCap = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    MaskFunction = new [] { PssMaskTypes.MGF1 },
                    HashPairs = hashPairs
                },
                new CapSigType
                {
                    Modulo = 3072,
                    MaskFunction = new [] { PssMaskTypes.SHAKE128 },
                    HashPairs = hashPairs
                }, 
                new CapSigType
                {
                    Modulo = 4096,
                    MaskFunction = new [] { PssMaskTypes.SHAKE256 },
                    HashPairs = hashPairs
                }
            };
            
            var algSpecs = new AlgSpecs[ParameterValidator.VALID_SIG_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpecs
                {
                    SigType = ParameterValidator.VALID_SIG_GEN_MODES[i],
                    ModuloCapabilities = modCap
                };
            }

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