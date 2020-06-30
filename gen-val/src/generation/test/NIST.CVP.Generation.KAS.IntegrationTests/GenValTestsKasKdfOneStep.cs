using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KAS_KDF.OneStep;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
	[TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKasKdfOneStep : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_KDF_OneStep_Sp800_56Cr1;
        public override string Algorithm => "KAS-KDF";
        public override string Mode => "OneStep";
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
                AuxFunctions = new[]
                {
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    }, 
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.SHA2_D224
                    },
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                },
                FixedInfoEncoding = new []
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l"
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
                AuxFunctions = new[]
                {
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    }, 
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.SHA2_D224
                    },
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.SHA2_D512
                    },
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA2_D512,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.SHA3_D512
                    },
                    new AuxFunction()
                    {
                        AuxFunctionName = KasKdfOneStepAuxFunction.HMAC_SHA3_D512,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                },
                FixedInfoEncoding = new []
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l"
            };

            return CreateRegistration(folderName, p);
        }
    }
}