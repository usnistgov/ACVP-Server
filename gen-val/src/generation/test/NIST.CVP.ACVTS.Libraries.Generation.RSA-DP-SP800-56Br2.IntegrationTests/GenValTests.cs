
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA_DecryptionPrimitive_SP800_56Br2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "decryptionPrimitive";
        public override string Revision { get; set; } = "Sp800-56Br2";
        public override AlgoMode AlgoMode => AlgoMode.RSA_DecryptionPrimitive_Sp800_56Br2;
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
        
        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                KeyFormat = new PrivateKeyModes[] { PrivateKeyModes.Standard },
                Modulo = new [] { 2048 },
                PublicExponentMode = PublicExponentModes.Fixed,
                PublicExponentValue = new BitString("010001")
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
                Modulo = new [] { 2048, 3072, 4096 }
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
            if (testCase.pt != null)
            {
                var bs = new BitString(testCase.pt.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.pt = bs.ToHex();
            }
        }
    }
}
