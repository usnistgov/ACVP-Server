using Autofac;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Common;

namespace NIST.CVP.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsAesKwp : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "AES-KWP";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.AES_KWP_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new KeyWrapWithPadding.AES.RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.testPassed != null)
            {
                testCase.testPassed = !(bool)testCase.testPassed;
            }

            // If TC has a cipherText, change it
            if (testCase.ct != null)
            {
                BitString bs = new BitString(testCase.ct.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.ct = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.pt != null)
            {
                BitString bs = new BitString(testCase.pt.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.pt = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KwCipher = ParameterValidator.VALID_KWCIPHER,
                KeyLen = new [] { 128 },
                PayloadLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 8, 64, 8))
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KwCipher = ParameterValidator.VALID_KWCIPHER,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PayloadLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 8, 4096, 8))
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
