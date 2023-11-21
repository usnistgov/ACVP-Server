using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.Hkdf;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKdaHkdf_56Cr2 : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KDA_HKDF_Sp800_56Cr2;
        public override string Algorithm => "KDA";
        public override string Mode => "HKDF";
        public override string Revision => "Sp800-56Cr2";
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

            // If TC has a dkms, change it
            if (testCase.dkms != null)
            {
                BitString bs = new BitString(testCase.dkms[0].ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.dkms[0] = bs.ToHex();
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
                L = 512,
                Z = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                Encoding = new[]
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                MacSaltMethods = new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                SaltLens = new MathDomain().AddSegment(new ValueDomainSegment(8))
                    .AddSegment(new ValueDomainSegment(16))
                    .AddSegment(new ValueDomainSegment(32))
                    .AddSegment(new ValueDomainSegment(64))
                    .AddSegment(new ValueDomainSegment(520))
                    .AddSegment(new ValueDomainSegment(528))
                    .AddSegment(new ValueDomainSegment(536))
                    .AddSegment(new ValueDomainSegment(542)),
                HmacAlg = new[] { HashFunctions.Sha1, HashFunctions.Sha2_d256, HashFunctions.Sha3_d256 },
                UsesHybridSharedSecret = false,
                PerformMultiExpansionTests = true,
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
                HmacAlg = EnumHelpers.GetEnumsWithoutDefault<HashFunctions>().Except(new[] { HashFunctions.Sha1 }).ToArray(),
                UsesHybridSharedSecret = true,
                AuxSharedSecretLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 112, 65536, 8)),
                PerformMultiExpansionTests = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
