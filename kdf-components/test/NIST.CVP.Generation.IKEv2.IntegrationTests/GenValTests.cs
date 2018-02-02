using NIST.CVP.Tests.Core.TestCategoryAttributes;
using Autofac;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;

namespace NIST.CVP.Generation.IKEv2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "IKEv2";

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

            if (testCase.sKeySeed != null)
            {
                var bs = new BitString(testCase.sKeySeed.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.sKeySeed = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                HashAlg = new [] {"sha-1"},
                InitiatorNonceLength = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                ResponderNonceLength = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                DerivedKeyingMaterialLength = new MathDomain().AddSegment(new ValueDomainSegment(256))
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var rand = new Random800_90();
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                HashAlg = new [] {"sha-1", "sha2-224"},
                InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8))
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var rand = new Random800_90();
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_DH, ParameterValidator.MAX_DH)),
                DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 512, ParameterValidator.MAX_DKM))
            };

            return CreateRegistration(folderName, p);
        }
    }
}
