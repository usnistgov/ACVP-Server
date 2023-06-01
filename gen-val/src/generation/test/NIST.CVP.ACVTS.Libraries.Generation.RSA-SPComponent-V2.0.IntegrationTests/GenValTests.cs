using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA_SPComponent_V2_0.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "signaturePrimitive";
        public override string Revision { get; set; } = "2.0";
        public override AlgoMode AlgoMode => AlgoMode.RSA_SignaturePrimitive_v2_0;
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
        
        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                KeyFormat = new PrivateKeyModes[] { PrivateKeyModes.Standard, PrivateKeyModes.Crt },
                Modulus = new [] { 2048, 3072, 4096 },
                PublicExponentMode = PublicExponentModes.Random
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
                IsSample = false,
                KeyFormat = new PrivateKeyModes[] { PrivateKeyModes.Standard, PrivateKeyModes.Crt },
                Modulus = new [] { 2048, 3072, 4096 },
                PublicExponentMode = PublicExponentModes.Fixed,
                PublicExponentValue = new BitString("010001")
            };

            return CreateRegistration(folderName, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            
            // Change test result
            if (testCase.testPassed != null)
            {
                testCase.testPassed = !((bool)testCase.testPassed);
            }
            
            // Change PT
            if (testCase.signature != null)
            {
                var bs = new BitString(testCase.signature.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.signature = bs.ToHex();
            }
        }
    }
}
