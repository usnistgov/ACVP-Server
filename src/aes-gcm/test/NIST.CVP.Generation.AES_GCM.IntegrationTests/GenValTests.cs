using NIST.CVP.Math;
using NUnit.Framework;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NIST.CVP.Math.Domain;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "GCM";

        public override AlgoMode AlgoMode => AlgoMode.AES_GCM;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.testPassed != null)
            {
                testCase.testPassed = true;
            }

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                BitString bs = new BitString(testCase.cipherText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.cipherText = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                BitString bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.plainText = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                ivLen = new MathDomain().AddSegment(new ValueDomainSegment(96)),
                ivGen = ParameterValidator.VALID_IV_GEN[0],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                aadLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                TagLen = new MathDomain().AddSegment(new ValueDomainSegment(64)),
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(0))
                    .AddSegment(new ValueDomainSegment(120)),
                ivLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(96))
                    .AddSegment(new ValueDomainSegment(120)),
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                aadLen = new MathDomain()
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
