using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.FFC.Fakes;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorValKdfKc : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _kasBuilder;
        private readonly ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _schemeBuilder;
        private readonly IShaFactory _shaFactory;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly IKdfFactory _kdfFactory;
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;
        private readonly TestCaseDispositionOption _intendedDisposition;

        public TestCaseGeneratorValKdfKc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder, 
            IShaFactory shaFactory, 
            IEntropyProviderFactory entropyProviderFactory, 
            IMacParametersBuilder macParametersBuilder, 
            IKdfFactory kdfFactory, 
            INoKeyConfirmationFactory noKeyConfirmationFactory, 
            TestCaseDispositionOption dispositionIntention
        )
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _shaFactory = shaFactory;
            _entropyProviderFactory = entropyProviderFactory;
            _macParametersBuilder = macParametersBuilder;
            _kdfFactory = kdfFactory;
            _noKeyConfirmationFactory = noKeyConfirmationFactory;
            _intendedDisposition = dispositionIntention;
        }

        public int NumberOfTestCasesToGenerate => 25;
        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            TestCase tc = new TestCase { TestCaseDisposition = _intendedDisposition };

            return Generate(group, tc);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            var macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(group.MacType)
                .WithMacLength(group.MacLen)
                .WithNonce(_entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random).GetEntropy(group.AesCcmNonceLen))
                .Build();

            if (group.AesCcmNonceLen != 0)
            {
                testCase.NonceAesCcm = macParameters.CcmNonce.GetDeepCopy();
            }

            var iutKeyConfirmationRole = group.KcRole;
            var serverIutKeyConfirmationRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(iutKeyConfirmationRole);

            // Handles Failures due to changed z, dkm, macData
            IKdfFactory kdfFactory = _kdfFactory;
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedZ)
            {
                testCase.FailureTest = true;
                kdfFactory = new FakeKdfFactory_BadZ(_shaFactory);
            }
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedDkm)
            {
                testCase.FailureTest = true;
                kdfFactory = new FakeKdfFactory_BadDkm(_shaFactory);
            }
            INoKeyConfirmationFactory noKeyConfirmationFactory = _noKeyConfirmationFactory;
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedMacData)
            {
                testCase.FailureTest = true;
                noKeyConfirmationFactory = new FakeNoKeyConfirmationFactory_BadMacData();
            }

            var uParty = _kasBuilder
                .WithPartyId(
                    group.KasRole == KeyAgreementRole.InitiatorPartyU
                        ? SpecificationMapping.IutId
                        : SpecificationMapping.ServerId
                )
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithKasDsaAlgoAttributes(group.KasDsaAlgoAttributes)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(group.HashAlg)
                        .WithKdfFactory(kdfFactory)
                        .WithNoKeyConfirmationFactory(noKeyConfirmationFactory)
                )
                .BuildKdfKc()
                .WithKeyLength(group.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(group.OiPattern)
                .WithKeyConfirmationRole(
                    group.KasRole == KeyAgreementRole.InitiatorPartyU
                        ? iutKeyConfirmationRole
                        : serverIutKeyConfirmationRole
                )
                .WithKeyConfirmationDirection(group.KcType)
                .Build();

            var vParty = _kasBuilder
                .WithPartyId(
                    group.KasRole == KeyAgreementRole.ResponderPartyV
                        ? SpecificationMapping.IutId
                        : SpecificationMapping.ServerId
                )
                .WithKeyAgreementRole(KeyAgreementRole.ResponderPartyV)
                .WithKasDsaAlgoAttributes(group.KasDsaAlgoAttributes)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(group.HashAlg)
                        .WithKdfFactory(kdfFactory)
                        .WithNoKeyConfirmationFactory(noKeyConfirmationFactory)
                )
                .BuildKdfKc()
                .WithKeyLength(group.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(group.OiPattern)
                .WithKeyConfirmationRole(
                    group.KasRole == KeyAgreementRole.ResponderPartyV
                        ? iutKeyConfirmationRole
                        : serverIutKeyConfirmationRole
                )
                .WithKeyConfirmationDirection(group.KcType)
                .Build();

            FfcDomainParameters dp = new FfcDomainParameters(
                group.P,
                group.Q,
                group.G
            );

            uParty.SetDomainParameters(dp);
            vParty.SetDomainParameters(dp);

            var uPartyPublic = uParty.ReturnPublicInfoThisParty();
            testCase.NonceNoKc = uPartyPublic.NoKeyConfirmationNonce;
            var vPartyPublic = vParty.ReturnPublicInfoThisParty();

            var serverKas = group.KasRole == KeyAgreementRole.InitiatorPartyU ? vParty : uParty;
            var iutKas = group.KasRole == KeyAgreementRole.InitiatorPartyU ? uParty : vParty;

            // Mangle the keys prior to running compute result, to create a "successful" result on bad keys.
            // IUT should pick up on bad private/public key information.
            TestCaseDispositionHelper.MangleKeys(
                testCase,
                _intendedDisposition,
                serverKas,
                iutKas
            );

            // Use the IUT kas for compute result
            KasResult iutResult = null;
            if (serverKas == uParty)
            {
                iutResult = vParty.ComputeResult(uPartyPublic);
            }
            else
            {
                iutResult = uParty.ComputeResult(vPartyPublic);
            }
            
            // Set the test case up w/ the information from the kas instances
            TestCaseDispositionHelper.SetTestCaseInformationFromKasResults(group, testCase, serverKas, iutKas, iutResult);
            
            // Change data for failures that do not require a rerun of functions
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedOi)
            {
                testCase.FailureTest = true;
                testCase.OtherInfo[0] += 2;
            }
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedTag)
            {
                testCase.FailureTest = true;
                testCase.Tag[0] += 2;
            }

            // check for successful conditions w/ constraints.
            if (_intendedDisposition == TestCaseDispositionOption.SuccessLeadingZeroNibbleZ)
            {
                // No zero nibble in MSB
                if (testCase.Z[0] >= 0x10)
                {
                    // call generate again, until getting to a zero nibble MSB for Z
                    Generate(group, testCase);
                }
            }

            // check for successful conditions w/ constraints.
            if (_intendedDisposition == TestCaseDispositionOption.SuccessLeadingZeroNibbleDkm)
            {
                // No zero nibble in MSB
                if (testCase.Dkm[0] >= 0x10)
                {
                    // call generate again, until getting to a zero nibble MSB for Z
                    Generate(group, testCase);
                }
            }

            return new TestCaseGenerateResponse(testCase);
        }
    }
}