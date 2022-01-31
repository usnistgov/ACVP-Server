using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3.Ecc;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSscEcc_Sp800_56Ar3 : GenValTestsWithNoSample
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_ECC_SSC_Sp800_56Ar3;
        public override string Algorithm => "KAS-ECC-SSC";
        public override string Mode => null;
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
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                HashFunctionZ = HashFunctions.None,
                DomainParameterGenerationMethods = new[]
                {
                    KasDpGeneration.K233,
                    KasDpGeneration.K283,
                    KasDpGeneration.K409,
                },
                Scheme = new Schemes
                {
                    EccStaticUnified = new EccStaticUnified
                    {
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        }
                    },
                    EccFullMqv = new EccFullMqv
                    {
                        KasRole = new[]
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
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = false,
                HashFunctionZ = HashFunctions.Sha3_d512,
                DomainParameterGenerationMethods = new[]
                {
                    KasDpGeneration.K233
                },
                Scheme = new Schemes
                {
                    EccStaticUnified = new EccStaticUnified
                    {
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        }
                    },
                    EccFullMqv = new EccFullMqv
                    {
                        KasRole = new[]
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
