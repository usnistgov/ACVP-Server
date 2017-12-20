using Autofac;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using RSA_SigVer;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;

namespace NIST.CVP.Generation.RSA_SigVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "SigVer";

        public override Executable Generator => Program.Main;
        public override Executable Validator => RSA_SigVer_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            RSA_SigVer_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            RSA_SigVer_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            RSA_SigVer_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a result, change it
            if (testCase.result != null)
            {
                testCase.result = !((bool)testCase.result);
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var hashPair = new[]
            {
                new HashPair
                {
                    HashAlg = "sha2-256",
                    SaltLen = 5
                }
            };

            var modCap = new[]
            {
                new CapSigType
                {
                    Modulo = 2048,
                    HashPairs = hashPair
                }
            };

            var algSpec = new[]
            {
                new AlgSpecs
                {
                    SigType = "pss",
                    ModuloCapabilities = modCap
                }
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Capabilities = algSpec,
                IsSample = false,
                PubExpMode = "fixed",
                FixedPubExpValue = "010001"
            };

            return CreateRegistration(targetFolder, p);
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
                IsSample = false,
                Capabilities = algSpecs,
                PubExpMode = "fixed",
                FixedPubExpValue = "010001"
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var hashPairs = new HashPair[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < hashPairs.Length; i++)
            {
                hashPairs[i] = new HashPair
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = i + 8
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
                IsSample = false,
                Capabilities = algSpecs,
                PubExpMode = "random"
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
