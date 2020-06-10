using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3;
using NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ffc;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
	[TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSscFfc_Sp800_56Ar3 : GenValTestsWithNoSample
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_FFC_SSC_Sp800_56Ar3;
        public override string Algorithm => "KAS-FFC-SSC";
        public override string Mode => string.Empty;
        public override string Revision => "Sp800-56Ar3";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a z, change it
            if (testCase.z != null)
            {
                BitString bs = new BitString(testCase.z.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.z = bs.ToHex();
            }
            
            // If TC has a hashZ, change it
            if (testCase.hashZ != null)
            {
                BitString bs = new BitString(testCase.hashZ.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.hashZ = bs.ToHex();
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
                HashFunctionZ = HashFunctions.None,
                DomainParameterGenerationMethods = new[]
                {
                    KasDpGeneration.Ffdhe2048
                },
                Scheme = new Schemes()
                {
                    FfcMqv1 = new FfcMqv1()
                    {
                        KasRole = new []
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        }
                    },
                    FfcDhEphem = new FfcDhEphem()
                    {
                        KasRole = new []
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        }
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            return GetTestFileFewTestCases(folderName);
        }
        
        protected override string GetTestFileFewTestCasesNotSample(string folderName)
        {
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = false,
                HashFunctionZ = HashFunctions.Sha3_d512,
                DomainParameterGenerationMethods = new[]
                {
                    KasDpGeneration.Ffdhe2048,
                    KasDpGeneration.Fb
                },
                Scheme = new Schemes()
                {
                    FfcMqv1 = new FfcMqv1()
                    {
                        KasRole = new []
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        }
                    },
                    FfcDhEphem = new FfcDhEphem()
                    {
                        KasRole = new []
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        }
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}