using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKasIfc : GenValTestsWithNoSample
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_IFC_Sp800_56Br2;
        public override string Algorithm => "KAS-IFC";
        public override string Mode => null;
        public override string Revision => "Sp800-56Br2";
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
        public override IJsonConverterProvider JsonConverterProvider => new KasJsonConverterProvider();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has dkm, change it
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
            // If TC has a tag, change it
            if (testCase.tag != null)
            {
                BitString bs = new BitString(testCase.tag.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.tag = bs.ToHex();
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
                IutId = new BitString("123456ABCD"),
                PublicExponent = new BigInteger(65537),
                KeyGenerationMethods = new[]
                {
                    IfcKeyGenerationMethod.RsaKpg1_basic
                },
                Modulo = new[] { 2048 },
                Scheme = new Schemes
                {
                    Kas1_partyV_confirmation = new Kas1_partyV_confirmation
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods
                        {
                            OneStepKdf = new OneStepKdf
                            {
                                Encoding = new[] { FixedInfoEncoding.Concatenation },
                                AuxFunctions = new[]
                                {
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.KMAC_128,
                                        MacSaltMethods = new []{ MacSaltMethod.Default }
                                    }
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                            OneStepNoCounterKdf = new OneStepNoCounterKdf
                            {
                                Encoding = new[] { FixedInfoEncoding.Concatenation },
                                AuxFunctions = new[]
                                {
                                    new AuxFunctionNoCounter
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D512,
                                        MacSaltMethods = new []{ MacSaltMethod.Default },
                                        L = 256
                                    }
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                            TwoStepKdf = new TwoStepKdf
                            {
                                Capabilities = new[]
                                {
                                    new TwoStepCapabilities
                                    {
                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
                                        MacSaltMethods = new[] { MacSaltMethod.Random },
                                        CounterLength = new [] { 32 },
                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                                        MacMode = new [] { MacModes.HMAC_SHA3_224 },
                                        KdfMode = KdfModes.Feedback,
                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
                                        SupportsEmptyIv = false
                                    }
                                },
                            },
                        },
                        MacMethods = new MacMethods
                        {
                            Kmac128 = new MacOptionKmac128
                            {
                                KeyLen = 128,
                                MacLen = 224
                            },
                        }
                    },
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCasesNotSample(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = false,
                IutId = new BitString("123456ABCD"),
                PublicExponent = new BigInteger(65537),
                KeyGenerationMethods = new[]
                {
                    IfcKeyGenerationMethod.RsaKpg1_basic
                },
                Modulo = new[] { 2048 },
                Scheme = new Schemes
                {
                    Kas1_basic = new Kas1_basic
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods
                        {
                            OneStepKdf = new OneStepKdf
                            {
                                Encoding = new[] { FixedInfoEncoding.Concatenation },
                                AuxFunctions = new[]
                                {
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA1
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D256
                                    }
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                            TwoStepKdf = new TwoStepKdf
                            {
                                Capabilities = new[]
                                {
                                    new TwoStepCapabilities
                                    {
                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
                                        MacSaltMethods = new[] { MacSaltMethod.Random },
                                        CounterLength = new [] { 32 },
                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                                        MacMode = new [] { MacModes.HMAC_SHA1, MacModes.HMAC_SHA3_224 },
                                        KdfMode = KdfModes.Feedback,
                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
                                        SupportsEmptyIv = false
                                    }
                                },
                            },
                        },
                    },
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
                IutId = new BitString("123456ABCD"),
                PublicExponent = new BigInteger(65539),
                KeyGenerationMethods = new[]
                {
                    IfcKeyGenerationMethod.RsaKpg2_basic,
                    IfcKeyGenerationMethod.RsaKpg1_basic,
                    IfcKeyGenerationMethod.RsaKpg2_crt
                },
                Modulo = new[] { 2048 },
                Scheme = new Schemes
                {
                    Kas1_basic = new Kas1_basic
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods
                        {
                            OneStepKdf = new OneStepKdf
                            {
                                Encoding = new[] { FixedInfoEncoding.Concatenation },
                                AuxFunctions = new[]
                                {
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D224,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D256,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D384,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA2_D512,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA3_D224,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA3_D256,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA3_D384,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.SHA3_D512,
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA2_D224,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA2_D256,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA2_D384,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA2_D512,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA3_D224,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA3_D256,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA3_D384,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.HMAC_SHA3_D512,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.KMAC_128,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.KMAC_256,
                                        MacSaltMethods = new[] {MacSaltMethod.Default, MacSaltMethod.Random}
                                    },
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                        },
                    },
                    Kas2_basic = new Kas2_basic
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods
                        {
                            OneStepKdf = new OneStepKdf
                            {
                                Encoding = new[] { FixedInfoEncoding.Concatenation },
                                AuxFunctions = new[]
                                {
                                    new AuxFunction
                                    {
                                        AuxFunctionName = KdaOneStepAuxFunction.KMAC_128,
                                        MacSaltMethods = new []{ MacSaltMethod.Default }
                                    }
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                        },
                    },
                    Kas1_partyV_confirmation = new Kas1_partyV_confirmation
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods
                        {
                            // OneStepKdf = new OneStepKdf()
                            // {
                            //     Encoding = new[] { FixedInfoEncoding.Concatenation },
                            //     AuxFunctions = new[]
                            //     {
                            //         new AuxFunction()
                            //         {
                            //             AuxFunctionName = KdaOneStepAuxFunction.KMAC_128,
                            //             MacSaltMethods = new []{ MacSaltMethod.Default }
                            //         }
                            //     },
                            //     FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            // },
                            TwoStepKdf = new TwoStepKdf
                            {
                                Capabilities = new[]
                                {
                                    new TwoStepCapabilities
                                    {
                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
                                        MacSaltMethods = new[] { MacSaltMethod.Random },
                                        CounterLength = new [] { 32 },
                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                                        MacMode = new [] { MacModes.HMAC_SHA3_224 },
                                        KdfMode = KdfModes.Feedback,
                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
                                        SupportsEmptyIv = false
                                    }
                                },
                            },
                        },
                        MacMethods = new MacMethods
                        {
                            Kmac128 = new MacOptionKmac128
                            {
                                KeyLen = 128,
                                MacLen = 224
                            },
                        }
                    },
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
