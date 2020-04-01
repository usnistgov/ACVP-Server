using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KAS.Sp800_56Ar3;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsEcc_Sp800_56Ar3 : GenValTestsWithNoSample
    {
        public override AlgoMode AlgoMode => AlgoMode.KAS_ECC_Sp800_56Ar3;
        public override string Algorithm => "KAS-ECC";
        public override string Mode => string.Empty;
        public override string Revision => "Sp800-56Ar3";
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
            var p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                IutId = new BitString("123456ABCD"),
                Scheme = new Schemes()
                {
                    EccOnePassUnified = new EccOnePassUnified()
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods()
                        {
                            OneStepKdf = new OneStepKdf()
                            {
                                Encoding = new[] { FixedInfoEncoding.Concatenation },
                                AuxFunctions = new[]
                                {
                                    new AuxFunction()
                                    {
                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                                        MacSaltMethods = new []{ MacSaltMethod.Default }
                                    }
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                            TwoStepKdf = new TwoStepKdf()
                            {
                                Capabilities = new[]
                                {
                                    new TwoStepCapabilities()
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
                        KeyConfirmationMethod = new KeyConfirmationMethod()
                        {
                            MacMethods = new MacMethods()
                            {
                                Kmac128 = new MacOptionKmac128()
                                {
                                    KeyLen = 128,
                                    MacLen = 128
                                }
                            },
                            KeyConfirmationDirections = new[]
                            {
                                KeyConfirmationDirection.Unilateral, 
                                //KeyConfirmationDirection.Bilateral,
                            },
                            KeyConfirmationRoles = new[]
                            {
                                KeyConfirmationRole.Provider, 
                                KeyConfirmationRole.Recipient,
                            }
                        },
                    },
//                    EccOnePassMqv = new EccOnePassMqv()
//                    {
//                        L = 512,
//                        KasRole = new[]
//                        {
//                            KeyAgreementRole.InitiatorPartyU,
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new[] { FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new[]
//                                {
//                                    new AuxFunction()
//                                    {
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default }
//                                    }
//                                },
//                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
//                            },
//                            TwoStepKdf = new TwoStepKdf()
//                            {
//                                Capabilities = new[]
//                                {
//                                    new TwoStepCapabilities()
//                                    {
//                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
//                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
//                                        MacSaltMethods = new[] { MacSaltMethod.Random },
//                                        CounterLength = new [] { 32 },
//                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
//                                        MacMode = new [] { MacModes.HMAC_SHA3_224 },
//                                        KdfMode = KdfModes.Feedback,
//                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
//                                        SupportsEmptyIv = false
//                                    }
//                                },
//                            },
//                        },
//                        KeyConfirmationMethod = new KeyConfirmationMethod()
//                        {
//                            MacMethods = new MacMethods()
//                            {
//                                Kmac128 = new MacOptionKmac128()
//                                {
//                                    KeyLen = 128,
//                                    MacLen = 128
//                                }
//                            },
//                            KeyConfirmationDirections = new[] { KeyConfirmationDirection.Unilateral },
//                            KeyConfirmationRoles = new[] { KeyConfirmationRole.Provider, KeyConfirmationRole.Recipient }
//                        },
//                    },
//                    EccEphemeralUnified = new EccEphemeralUnified()
//                    {
//                        L = 512,
//                        KasRole = new[]
//                        {
//                            KeyAgreementRole.InitiatorPartyU,
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new[] { FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new[]
//                                {
//                                    new AuxFunction()
//                                    {
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default }
//                                    }
//                                },
//                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
//                            },
//                            TwoStepKdf = new TwoStepKdf()
//                            {
//                                Capabilities = new[]
//                                {
//                                    new TwoStepCapabilities()
//                                    {
//                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
//                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
//                                        MacSaltMethods = new[] { MacSaltMethod.Random },
//                                        CounterLength = new [] { 32 },
//                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
//                                        MacMode = new [] { MacModes.HMAC_SHA3_224 },
//                                        KdfMode = KdfModes.Feedback,
//                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
//                                        SupportsEmptyIv = false
//                                    }
//                                },
//                            },
//                        },
//                    },
//                    EccOnePassDh = new EccOnePassDh()
//                    {
//                        L = 512,
//                        KasRole = new[]
//                        {
//                            KeyAgreementRole.InitiatorPartyU,
//                            KeyAgreementRole.ResponderPartyV
//                        },
//                        KdfMethods = new KdfMethods()
//                        {
//                            OneStepKdf = new OneStepKdf()
//                            {
//                                Encoding = new[] { FixedInfoEncoding.Concatenation },
//                                AuxFunctions = new[]
//                                {
//                                    new AuxFunction()
//                                    {
//                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
//                                        MacSaltMethods = new []{ MacSaltMethod.Default }
//                                    }
//                                },
//                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
//                            },
//                            TwoStepKdf = new TwoStepKdf()
//                            {
//                                Capabilities = new[]
//                                {
//                                    new TwoStepCapabilities()
//                                    {
//                                        Encoding = new[] { FixedInfoEncoding.Concatenation },
//                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
//                                        MacSaltMethods = new[] { MacSaltMethod.Random },
//                                        CounterLength = new [] { 32 },
//                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
//                                        MacMode = new [] { MacModes.HMAC_SHA3_224 },
//                                        KdfMode = KdfModes.Feedback,
//                                        FixedDataOrder = new []{ CounterLocations.AfterFixedData },
//                                        SupportsEmptyIv = false
//                                    }
//                                },
//                            },
//                        },
//                        KeyConfirmationMethod = new KeyConfirmationMethod()
//                        {
//                            MacMethods = new MacMethods()
//                            {
//                                Kmac128 = new MacOptionKmac128()
//                                {
//                                    KeyLen = 128,
//                                    MacLen = 128
//                                }
//                            },
//                            KeyConfirmationDirections = new[] { KeyConfirmationDirection.Unilateral },
//                            KeyConfirmationRoles = new[] { KeyConfirmationRole.Provider, KeyConfirmationRole.Recipient }
//                        },
//                    },
                },
                DomainParameterGenerationMethods = new[] { KasDpGeneration.P192 }
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
                IutId = new BitString("123456ABCD"),
                Scheme = new Schemes()
                {
                    EccOnePassUnified = new EccOnePassUnified()
                    {
                        L = 512,
                        KasRole = new[]
                        {
                            KeyAgreementRole.InitiatorPartyU,
                            KeyAgreementRole.ResponderPartyV
                        },
                        KdfMethods = new KdfMethods()
                        {
                            OneStepKdf = new OneStepKdf()
                            {
                                Encoding = new[] {FixedInfoEncoding.Concatenation},
                                AuxFunctions = new[]
                                {
                                    new AuxFunction()
                                    {
                                        AuxFunctionName = KasKdfOneStepAuxFunction.KMAC_128,
                                        MacSaltMethods = new[] {MacSaltMethod.Default}
                                    }
                                },
                                FixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo"
                            },
                            TwoStepKdf = new TwoStepKdf()
                            {
                                Capabilities = new[]
                                {
                                    new TwoStepCapabilities()
                                    {
                                        Encoding = new[] {FixedInfoEncoding.Concatenation},
                                        FixedInfoPattern = "l||label||uPartyInfo||vPartyInfo||context",
                                        MacSaltMethods = new[] {MacSaltMethod.Random},
                                        CounterLength = new[] {32},
                                        SupportedLengths = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                                        MacMode = new[] {MacModes.HMAC_SHA3_224},
                                        KdfMode = KdfModes.Feedback,
                                        FixedDataOrder = new[] {CounterLocations.AfterFixedData},
                                        SupportsEmptyIv = false
                                    }
                                },
                            },
                        },
                        KeyConfirmationMethod = new KeyConfirmationMethod()
                        {
                            MacMethods = new MacMethods()
                            {
                                Kmac128 = new MacOptionKmac128()
                                {
                                    KeyLen = 128,
                                    MacLen = 128
                                }
                            },
                            KeyConfirmationDirections = new[]
                            {
                                KeyConfirmationDirection.Unilateral,
                                //KeyConfirmationDirection.Bilateral,
                            },
                            KeyConfirmationRoles = new[]
                            {
                                KeyConfirmationRole.Provider,
                                KeyConfirmationRole.Recipient,
                            }
                        },
                    }
                },
                DomainParameterGenerationMethods = new[] { KasDpGeneration.P192 }
            };

            return CreateRegistration(folderName, p);
        }
    }
}