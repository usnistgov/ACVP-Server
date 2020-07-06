using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KAS_KDF.TwoStep;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
	[TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKasKdfTwoStep : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_KDF_TwoStep_Sp800_56Cr1;
        public override string Algorithm => "KAS-KDF";
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
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                L = 512,
                Z = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                Capabilities = new TwoStepCapabilities[]
                {
                    new TwoStepCapabilities()
                    {
                        FixedInfoEncoding = new []
                        {
                            FixedInfoEncoding.Concatenation
                        },
                        FixedInfoPattern = "uPartyInfo||vPartyInfo||l",
                        MacSaltMethods = new[] { MacSaltMethod.Random },
                        CounterLength = new [] { 32 },
                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                        MacMode = new [] { MacModes.HMAC_SHA3_224 },
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
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                L = 512,
                Z = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                Capabilities = new TwoStepCapabilities[]
                {
                    new TwoStepCapabilities()
                    {
                        FixedInfoEncoding = new []
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