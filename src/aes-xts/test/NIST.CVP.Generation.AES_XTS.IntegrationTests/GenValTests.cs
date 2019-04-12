using NIST.CVP.Common;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math.Domain;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.AES_XTS.v1_0;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XTS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "AES-XTS";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.AES_XTS_v1_0;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            
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

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                KeyLen = new[] { 128 },
                Direction = new[] { "encrypt", "decrypt" },
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512)),
                TweakModes = new[] { "hex", "number" },
                IsSample = false
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
                KeyLen = new[] { 128, 256 },
                Direction = new[] { "encrypt", "decrypt" },
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536)),
                TweakModes = new[] { "hex", "number" },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
