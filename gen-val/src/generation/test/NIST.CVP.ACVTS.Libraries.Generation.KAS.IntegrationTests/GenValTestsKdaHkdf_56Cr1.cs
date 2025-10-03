using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr1.Hkdf;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKdaHkdf_56Cr1 : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KDA_HKDF_Sp800_56Cr1;
        public override string Algorithm => "KDA";
        public override string Mode => "HKDF";
        public override string Revision => "Sp800-56Cr1";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a dkm, change it
            if (testCase.dkm != null)
            {
                BitString bs = new BitString(testCase.dkm.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.dkm = bs.ToHex();
            }

            // If TC has a result, change it
            if (testCase.testPassed != null)
            {
                if (testCase.testPassed == true)
                {
                    testCase.testPassed = false;
                }
                else
                {
                    testCase.testPassed = true;
                }
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                L = 256,
                Z = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                Encoding = new[]
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                MacSaltMethods = new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                HmacAlg = new[] { HashFunctions.Sha1, HashFunctions.Sha2_d256, HashFunctions.Sha3_d256 }
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
                IsSample = true,
                L = 1024,
                Z = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 224, 65536, 8)),
                Encoding = new[]
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                MacSaltMethods = new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                HmacAlg = EnumHelpers.GetEnumsWithoutDefault<HashFunctions>()
                                     .Except(new[] { HashFunctions.Sha1, HashFunctions.Shake_d128, HashFunctions.Shake_d256 }).ToArray()
            };

            return CreateRegistration(folderName, p);
        }
    }
}
