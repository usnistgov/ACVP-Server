using Autofac;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsEccComponent : GenValTestsSingleRunnerBase
    {
        private readonly Random800_90 _random = new Random800_90();

        public override string Algorithm => "KAS-ECC";

        public override string Mode => "Component";
        public override string RunnerAlgorithm => "KAS";
        public override string RunnerMode => "EccComponent";

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] { $"{Algorithm}{Mode}" };

            AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<EccComponent.Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            return GetTestFileFewTestCases(folderName);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            EccComponent.Parameters p = new EccComponent.Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Curves = new string[] { "p-192" },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            EccComponent.Parameters p = new EccComponent.Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Curves = new string[] { "p-192", "k-163", "b-163" },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
        
        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a z, change it
            if (testCase.z != null)
            {
                BitString bs = new BitString(testCase.z.ToString());
                bs = _random.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.z = bs.ToHex();
            }
        }
    }
}