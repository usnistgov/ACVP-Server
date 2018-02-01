using Autofac;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "CMAC";
        public override string Mode { get; } = "AES";

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] {"CMAC-AES"};
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.result != null)
            {
                if (testCase.result == "fail")
                {
                    testCase.result = "pass";
                }
                else
                {
                    testCase.result = "fail";
                }
            }

            // If TC has a mac, change it
            if (testCase.mac != null)
            {
                BitString bs = new BitString(testCase.mac.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.mac = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = "CMAC-AES",
                Mode = Mode,
                Direction = new[] {"gen"},
                KeyLen = new[] {128},
                MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                MacLen = new MathDomain().AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(127)),
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-AES",
                Mode = Mode,
                Direction = new[] { "gen", "ver" },
                KeyLen = new[] { 128 },
                MsgLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                MacLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(127)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Random800_90 random = new Random800_90();
            
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-AES",
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new[] { 128, 192, 256 },
                MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 0, 65536, 8)),
                MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 64, 128, 8)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
