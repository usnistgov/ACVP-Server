using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "TDES-CTR";
        public override string Mode => string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CTR_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = new[] { "encrypt", "decrypt" },
                KeyingOption = new[] { 1, 2 },
                IncrementalCounter = false,
                OverflowCounter = false,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 64, 8)),
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = new[] { "encrypt", "decrypt" },
                KeyingOption = new[] { 1, 2 },
                IncrementalCounter = false,
                OverflowCounter = true,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1, 64)),
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a cipherText, change it
            if (testCase.ct != null)
            {
                var bs = new BitString(testCase.ct.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.ct = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.pt != null)
            {
                var bs = new BitString(testCase.pt.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.pt = bs.ToHex();
            }
        }
    }
}
