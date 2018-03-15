using Autofac;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.IntegrationTests
{
    [TestFixture]
    public abstract class GenValHmacBase : GenValTestsSingleRunnerBase
    {
        public abstract override string Algorithm { get; }
        public override string Mode { get; } = "";

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }
        
        protected override void OverrideRegistrationValFakeException()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeVectorSetDeserializerException<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a mac, change it
            if (testCase.mac != null)
            {
                var bs = new BitString(testCase.mac.ToString());
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
            return GetTestFileFewTestCases(targetFolder);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                KeyLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 32, 8)),
                MacLen = new MathDomain().AddSegment(new ValueDomainSegment(64)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Random800_90 random = new Random800_90();

            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                KeyLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 8, 2048, 8)),
                MacLen = new MathDomain().AddSegment(new RangeDomainSegment(random, 80, 160, 8)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
