using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStepNoCounter;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKdaOneStepNoCounter_56Cr2 : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.KDA_OneStepNoCounter_Sp800_56Cr2;
        public override string Algorithm => "KDA";
        public override string Mode => "OneStepNoCounter";
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
                Z = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                AuxFunctions = new[]
                {
                    new AuxFunctionNoCounter
                    {
                        L = 256,
                        AuxFunctionName = KdaOneStepAuxFunction.KMAC_128,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 160,
                        AuxFunctionName = KdaOneStepAuxFunction.SHA1
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 160,
                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA1,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default
                        }
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 224,
                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D224
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 224,
                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA2_D224,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                },
                Encoding = new[]
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||t||l"
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
                Z = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 224, 65536, 8)),
                AuxFunctions = new[]
                {
                    new AuxFunctionNoCounter
                    {
                        L = 256,
                        AuxFunctionName = KdaOneStepAuxFunction.KMAC_128,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 224,
                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D224
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 224,
                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA2_D224,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 256,
                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D512
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 256,
                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA2_D512,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 256,
                        AuxFunctionName = KdaOneStepAuxFunction.SHA3_D512
                    },
                    new AuxFunctionNoCounter
                    {
                        L = 256,
                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA3_D512,
                        MacSaltMethods = new[]
                        {
                            MacSaltMethod.Default,
                            MacSaltMethod.Random
                        }
                    },
                },
                Encoding = new[]
                {
                    FixedInfoEncoding.Concatenation
                },
                FixedInfoPattern = "uPartyInfo||vPartyInfo||l"
            };

            return CreateRegistration(folderName, p);
        }
    }
}
