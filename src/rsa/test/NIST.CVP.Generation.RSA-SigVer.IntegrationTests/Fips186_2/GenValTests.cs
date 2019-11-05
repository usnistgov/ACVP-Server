using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.RSA.v1_0.SigVer;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using ParameterValidator = NIST.CVP.Generation.RSA.v1_0.LegacySigVer.ParameterValidator;
using RegisterInjections = NIST.CVP.Generation.RSA.v1_0.LegacySigVer.RegisterInjections;

namespace NIST.CVP.Generation.RSA_SigVer.IntegrationTests.Fips186_2
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "LegacySigVer";

        public override AlgoMode AlgoMode => AlgoMode.RSA_SigVer_Fips186_2;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a result, change it
            if (testCase.testPassed != null)
            {
                testCase.testPassed = !(bool) testCase.testPassed;
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var hashPairs = new []
            {
                new HashPair
                {
                    HashAlg = "sha2-224",
                    SaltLen = 16
                }
            };

            var modCap = new []
            {
                new CapSigType
                {
                    Modulo = 2048,
                    HashPairs = hashPairs   
                }
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
                PubExpMode = "random",
                KeyFormat = "standard"
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var hashPairs = new HashPair[2];
            for (var i = 0; i < hashPairs.Length; i++)
            {
                hashPairs[i] = new HashPair
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = i + 1
                };
            }

            var modCap = new CapSigType[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < modCap.Length; i++)
            {
                modCap[i] = new CapSigType
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashPairs = hashPairs
                };
            }

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
                PubExpMode = "random",
                KeyFormat = "crt"
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
