using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.TDES;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsTdes : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ACVP-TDES-KW";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.TDES_KW_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


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
                KeyLen = new[] { 192 },
                PayloadLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 64))
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
                KeyLen = new[] { 192 },
                PayloadLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 128, 4096, 64))
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
