using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "AES";
        public override string Mode => "CTR";

        public override AlgoMode AlgoMode => AlgoMode.AES_CTR;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] { "encrypt" },
                KeyLen = new[] { 128, 256 },
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 128, 8)),
                IsSample = true,
                IncrementalCounter = false,
                OverflowCounter = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 128)),
                IsSample = true,    // needs sample to complete deferred cases
                IncrementalCounter = true,
                OverflowCounter = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                var bs = new BitString(testCase.cipherText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.cipherText = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                var bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.plainText = bs.ToHex();
            }
        }
    }
}
