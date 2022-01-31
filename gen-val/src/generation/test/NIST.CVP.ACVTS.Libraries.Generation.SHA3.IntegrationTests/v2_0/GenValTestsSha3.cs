using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.IntegrationTests.v2_0
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha3 : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "SHA3-224";
        public override string Mode { get; } = null;
        public override string Revision { get; set; } = "2.0";

        public override AlgoMode AlgoMode => AlgoMode.SHA3_224_v2_0;

        public override IRegisterInjections RegistrationsGenVal { get; } = new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.md != null)
            {
                var bs = new BitString(testCase.md.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.md = bs.ToHex();
            }

            if (testCase.resultsArray != null)
            {
                var bsDigest = new BitString(testCase.resultsArray[0].md.ToString());
                bsDigest = rand.GetDifferentBitStringOfSameSize(bsDigest);
                testCase.resultsArray[0].md = bsDigest.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 1024, 8)),
                PerformLargeDataTest = new[] { 1, 2 },
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 65536)),
                PerformLargeDataTest = ParameterValidator.VALID_LARGE_DATA_SIZES,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }
    }
}
