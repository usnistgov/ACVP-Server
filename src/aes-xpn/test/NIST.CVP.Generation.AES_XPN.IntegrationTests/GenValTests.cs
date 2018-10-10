using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XPN.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "XPN";

        public override AlgoMode AlgoMode => AlgoMode.AES_XPN;

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
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                SaltGen = ParameterValidator.VALID_SALT_GEN[1],
                aadLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                TagLen = new MathDomain().AddSegment(new ValueDomainSegment(64)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-XPN",
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PtLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(256)),
                ivGen = ParameterValidator.VALID_IV_GEN[1],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                SaltGen = ParameterValidator.VALID_SALT_GEN[1],
                aadLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(120)),
                TagLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(64))
                    .AddSegment(new ValueDomainSegment(128)),
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
