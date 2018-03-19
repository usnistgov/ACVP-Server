using NIST.CVP.Generation.Core.Tests;
using Autofac;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "srtp";

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] {Mode};
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeVectorSetDeserializerException<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.srtpKe != null)
            {
                var bs = new BitString(testCase.srtpKe.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.srtpKe = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                AesKeyLength = new [] {128},
                KdrExponent = new [] {1},
                SupportsZeroKdr = false,
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                AesKeyLength = new [] {128, 192},
                KdrExponent = new [] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
                SupportsZeroKdr = true,
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                AesKeyLength = ParameterValidator.VALID_AES_KEY_LENGTHS,
                KdrExponent = ParameterValidator.VALID_KDR_EXPONENTS,
                SupportsZeroKdr = true,
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }
    }
}
