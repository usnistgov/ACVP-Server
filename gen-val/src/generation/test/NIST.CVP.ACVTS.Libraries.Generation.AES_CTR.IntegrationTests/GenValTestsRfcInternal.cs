using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsRfcInternal : GenValTestsWithNoSample
    {
        public override string Algorithm => "ACVP-AES-CTR";
        public override string Mode => string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.AES_CTR_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Conformances = new[] { "RFC3686" },
                IvGenMode = IvGenModes.Internal,
                Direction = new[] { "encrypt" },
                KeyLen = new[] { 128, 256 },
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 128, 8)),
                IsSample = true,
                IncrementalCounter = true,
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
                Revision = Revision,
                Conformances = new[] { "RFC3686" },
                IvGenMode = IvGenModes.Internal,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 128)),
                IsSample = true,    // needs sample to complete deferred cases
                IncrementalCounter = true,
                OverflowCounter = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCasesNotSample(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Conformances = new[] { "RFC3686" },
                IvGenMode = IvGenModes.Internal,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 128)),
                IsSample = false,
                IncrementalCounter = true,
                OverflowCounter = true
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
