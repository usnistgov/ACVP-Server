using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr1.TwoStep;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKdaTwoStep_56Cr1 : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KDA_TwoStep_Sp800_56Cr1;
        public override string Algorithm => "KDA";
        public override string Mode => "TwoStep";
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
                L = 512,
                Z = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                Capabilities = new[]
                {
                    new TwoStepCapabilities
                    {
                        Encoding = new []
                        {
                            FixedInfoEncoding.Concatenation
                        },
                        FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                        MacSaltMethods = new[] { MacSaltMethod.Random },
                        CounterLength = new [] { 32 },
                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                        MacMode = new [] { MacModes.HMAC_SHA1, MacModes.HMAC_SHA3_224 },
                        KdfMode = KdfModes.Feedback,
                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
                        SupportsEmptyIv = false
                    }
                }
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
                L = 512,
                Z = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 224, 65536, 8)),
                Capabilities = new[]
                {
                    new TwoStepCapabilities
                    {
                        Encoding = new []
                        {
                            FixedInfoEncoding.Concatenation
                        },
                        FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                        MacSaltMethods = new[] { MacSaltMethod.Random, MacSaltMethod.Default },
                        CounterLength = new [] { 32 },
                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                        MacMode = new [] { MacModes.HMAC_SHA3_224, MacModes.HMAC_SHA512 },
                        KdfMode = KdfModes.Feedback,
                        FixedDataOrder = new []{ CounterLocations.AfterFixedData, CounterLocations.BeforeIterator },
                        SupportsEmptyIv = false
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
