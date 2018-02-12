using Autofac;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
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

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
        }

        //[Test]
        //public void QuickStartUpLots()
        //{
        //    var targetFolder = Path.Combine(TestPath, "QuickStart");
        //    RunValidation(targetFolder);

        //    // Get object for the validation.json
        //    var dp = new DynamicParser();
        //    var parsedValidation = dp.Parse($@"{targetFolder}\validation.json");
        //    Assert.AreEqual(EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed), parsedValidation.ParsedObject.disposition.ToString(), "disposition");
        //}

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            
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

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var caps = new Capability[1];
            caps[0] = new Capability
            {
                Modulo = 2048,
                HashAlgs = new[] { "sha2-224" },
                PrimeTests = new[] { "tblc2" }
            };

            var algSpecs = new AlgSpec[ParameterValidator.VALID_KEY_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpec
                {
                    RandPQ = ParameterValidator.VALID_KEY_GEN_MODES[i],
                    Capabilities = caps
                };
            }

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                InfoGeneratedByServer = true,
                IsSample = true,
                PubExpMode = "random",
                KeyFormat = "standard",
                AlgSpecs = algSpecs
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new Capability[1];
            caps[0] = new Capability
            {
                Modulo = 2048,
                HashAlgs = new[] { "sha2-224" },
                PrimeTests = new[] { "tblc2" }
            };

            var algSpecs = new AlgSpec[4];
            algSpecs[0] = new AlgSpec
            {
                RandPQ = "b.3.2",
                Capabilities = caps
            };

            algSpecs[1] = new AlgSpec
            {
                RandPQ = "b.3.4",
                Capabilities = caps
            };

            algSpecs[2] = new AlgSpec
            {
                RandPQ = "b.3.5",
                Capabilities = caps
            };

            algSpecs[3] = new AlgSpec
            {
                RandPQ = "b.3.6",
                Capabilities = caps
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                InfoGeneratedByServer = true,
                IsSample = true,
                PubExpMode = "fixed",
                FixedPubExp = "010001",
                KeyFormat = "standard",
                AlgSpecs = algSpecs
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new Capability[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new Capability
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashAlgs = new string[] { "sha2-224" },
                    PrimeTests = ParameterValidator.VALID_PRIME_TESTS
                };
            }

            var algSpecs = new AlgSpec[ParameterValidator.VALID_KEY_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpec
                {
                    RandPQ = ParameterValidator.VALID_KEY_GEN_MODES[i],
                    Capabilities = caps
                };
            }

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                InfoGeneratedByServer = false,
                IsSample = true,
                PubExpMode = "random",
                AlgSpecs = algSpecs,
                KeyFormat = "crt"
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
