using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KAS_KDF.Hkdf;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
	[TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKasKdfHkdf : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_KDF_OneStep_Sp800_56Cr1;
        public override string Algorithm => "KAS-KDF";
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
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                L = 512,
                Z = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                Encoding = new []
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random },
                HmacAlg = new []{ HashFunctions.Sha2_d256, HashFunctions.Sha3_d256 }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                L = 1024,
                Z = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 224, 65336, 8)),
                Encoding = new []
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                MacSaltMethods = new []{ MacSaltMethod.Default, MacSaltMethod.Random },
                HmacAlg = EnumHelpers.GetEnumsWithoutDefault<HashFunctions>().Except(new []{ HashFunctions.Sha1 }).ToArray()
            };

            return CreateRegistration(folderName, p);
        }
    }
}