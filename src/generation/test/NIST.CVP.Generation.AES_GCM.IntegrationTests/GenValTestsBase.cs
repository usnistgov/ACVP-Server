using NIST.CVP.Common;
using NIST.CVP.Generation.AES_GCM.v1_0;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public abstract class GenValTestsBase : GenValTestsSingleRunnerBase
    {
        public override string Mode { get; } = string.Empty;

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

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.ct = bs.ToHex();
            }

            // If TC has a tag, change it
            if (testCase.tag != null)
            {
                BitString bs = new BitString(testCase.tag.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.tag = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.pt != null)
            {
                BitString bs = new BitString(testCase.pt.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.pt = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                IvLen = new MathDomain().AddSegment(new ValueDomainSegment(96)),
                IvGen = ParameterValidator.VALID_IV_GEN[0],
                IvGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                AadLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                TagLen = new MathDomain().AddSegment(new ValueDomainSegment(64)),
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PayloadLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(0))
                    .AddSegment(new ValueDomainSegment(120)),
                IvLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(96))
                    .AddSegment(new ValueDomainSegment(120)),
                IvGen = ParameterValidator.VALID_IV_GEN[1],
                IvGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                AadLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(0))
                    .AddSegment(new ValueDomainSegment(120)),
                TagLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(ParameterValidator.VALID_TAG_LENGTHS.First()))
                    .AddSegment(new ValueDomainSegment(ParameterValidator.VALID_TAG_LENGTHS.Last())),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
