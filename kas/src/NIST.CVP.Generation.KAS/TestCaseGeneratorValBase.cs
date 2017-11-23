using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS
{
    public abstract class TestCaseGeneratorValBase<
        TTestGroup, 
        TTestCase, 
        TKasDsaAlgoAttributes, 
        TDomainParameters, 
        TKeyPair
    > : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TKasDsaAlgoAttributes>
        where TTestCase : TestCaseBase, new()
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {

        protected readonly IKasBuilder<
            TKasDsaAlgoAttributes, 
            OtherPartySharedInformation<
                TDomainParameters, 
                TKeyPair
            >, 
            TDomainParameters, 
            TKeyPair
        > _kasBuilder;
        protected readonly ISchemeBuilder<
            TKasDsaAlgoAttributes, 
            OtherPartySharedInformation<
                TDomainParameters, 
                TKeyPair
            >, 
            TDomainParameters, 
            TKeyPair
        > _schemeBuilder;
        protected readonly IShaFactory _shaFactory;
        protected readonly IEntropyProviderFactory _entropyProviderFactory;
        protected readonly IMacParametersBuilder _macParametersBuilder;
        protected readonly IKdfFactory _kdfFactory;
        protected readonly IKeyConfirmationFactory _keyConfirmationFactory;
        protected readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;
        protected TestCaseDispositionOption _intendedDisposition;

        protected TestCaseGeneratorValBase(
            IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> kasBuilder,
            ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> schemeBuilder,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            TestCaseDispositionOption intendedDisposition
        )
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _shaFactory = shaFactory;
            _entropyProviderFactory = entropyProviderFactory;
            _macParametersBuilder = macParametersBuilder;
            _kdfFactory = kdfFactory;
            _noKeyConfirmationFactory = noKeyConfirmationFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
            _intendedDisposition = intendedDisposition;
        }

        public int NumberOfTestCasesToGenerate => 25;
        public TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample)
        {
            var testCase = new TTestCase()
            {
                TestCaseDisposition = _intendedDisposition
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
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
            var serverKeyConfirmationRole =
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
            IKeyConfirmationFactory keyConfirmationFactory = _keyConfirmationFactory;
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedMacData)
            {
                testCase.FailureTest = true;
                keyConfirmationFactory = new FakeKeyConfirmationFactory_BadMacData();
            }

            var uParty = GetKasInstance(
                KeyAgreementRole.InitiatorPartyU,
                group.KasRole == KeyAgreementRole.InitiatorPartyU
                    ? iutKeyConfirmationRole
                    : serverKeyConfirmationRole,
                macParameters,
                group,
                testCase,
                group.KasRole == KeyAgreementRole.InitiatorPartyU
                    ? SpecificationMapping.IutId
                    : SpecificationMapping.ServerId,
                kdfFactory,
                noKeyConfirmationFactory,
                keyConfirmationFactory
            );

            var vParty = GetKasInstance(
                KeyAgreementRole.ResponderPartyV,
                group.KasRole == KeyAgreementRole.ResponderPartyV
                    ? iutKeyConfirmationRole
                    : serverKeyConfirmationRole,
                macParameters,
                group,
                testCase,
                group.KasRole == KeyAgreementRole.ResponderPartyV
                    ? SpecificationMapping.IutId
                    : SpecificationMapping.ServerId,
                kdfFactory,
                noKeyConfirmationFactory,
                keyConfirmationFactory
            );

            TDomainParameters domainParameters = GetDomainParameters(group);

            uParty.SetDomainParameters(domainParameters);
            vParty.SetDomainParameters(domainParameters);

            var uPartyPublic = uParty.ReturnPublicInfoThisParty();
            testCase.NonceNoKc = uPartyPublic.NoKeyConfirmationNonce;
            var vPartyPublic = vParty.ReturnPublicInfoThisParty();

            var serverKas = group.KasRole == KeyAgreementRole.InitiatorPartyU ? vParty : uParty;
            var iutKas = group.KasRole == KeyAgreementRole.InitiatorPartyU ? uParty : vParty;

            // Mangle the keys prior to running compute result, to create a "successful" result on bad keys.
            // IUT should pick up on bad private/public key information.
            MangleKeys(
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

            SetTestCaseInformationFromKasResult(group, testCase, serverKas, iutKas, iutResult);

            // Change data for failures that do not require a rerun of functions
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedOi)
            {
                testCase.FailureTest = true;
                testCase.OtherInfo[0] += 2;
            }
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedTag)
            {
                if (testCase.Tag != null)
                {
                    testCase.FailureTest = true;
                    testCase.Tag[0] += 2;
                }
                if (testCase.HashZ != null)
                {
                    testCase.FailureTest = true;
                    testCase.HashZ[0] += 2;
                }
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
        
        /// <summary>
        /// Gets the KAS instance based on the provided parameters.
        /// </summary>
        /// <param name="partyRole">The party's role.</param>
        /// <param name="partyKcRole">The party's key confirmation role.</param>
        /// <param name="macParameters">The MAC parameters.</param>
        /// <param name="group">The test group.</param>
        /// <param name="testCase">The test case.</param>
        /// <param name="partyId">The party's identifier.</param>
        /// <param name="kdfFactory">The <see cref="IKdfFactory"/> implementation (can be real or fake)</param>
        /// <param name="noKeyConfirmationFactory">The <see cref="INoKeyConfirmationFactory"/> implementation (can be real or fake)</param>
        /// <param name="keyConfirmationFactory">The <see cref="IKeyConfirmationFactory"/> implementation (can be real or fake)</param>
        /// <returns></returns>
        protected abstract IKas<
            TKasDsaAlgoAttributes,
            OtherPartySharedInformation<
                TDomainParameters,
                TKeyPair
            >,
            TDomainParameters,
            TKeyPair
        > GetKasInstance(
            KeyAgreementRole partyRole,
            KeyConfirmationRole partyKcRole,
            MacParameters macParameters,
            TTestGroup @group,
            TTestCase testCase,
            BitString partyId,
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory
        );

        /// <summary>
        /// Get domain parameters from the group
        /// </summary>
        /// <param name="testGroup">The test group</param>
        /// <returns></returns>
        public abstract TDomainParameters GetDomainParameters(TTestGroup testGroup);

        /// <summary>
        /// Mangles partyU/partyV ephemeral/static keys dependant on the <see cref="intendedDisposition"/>
        /// </summary>
        /// <param name="testCase">The <see cref="TTestCase"/></param>
        /// <param name="intendedDisposition">The intended outcome of the test</param>
        /// <param name="serverKas">THe server party's KAS instance</param>
        /// <param name="iutKas">THe IUT party's KAS instance</param>
        protected abstract void MangleKeys(
            TTestCase testCase,
            TestCaseDispositionOption intendedDisposition,
            IKas<
                TKasDsaAlgoAttributes, 
                OtherPartySharedInformation<
                    TDomainParameters, 
                    TKeyPair
                >, 
                TDomainParameters, 
                TKeyPair
            > serverKas,
            IKas<
                TKasDsaAlgoAttributes, 
                OtherPartySharedInformation<
                    TDomainParameters, 
                    TKeyPair
                >, 
                TDomainParameters, 
                TKeyPair
            > iutKas
        );

        /// <summary>
        /// Sets the KAS instance's generated information on the test case.
        /// </summary>
        /// <param name="group">The test group.</param>
        /// <param name="testCase">The test case in which to have its information set.</param>
        /// <param name="serverKas">The server's instance of KAS.</param>
        /// <param name="iutKas">The IUT's instance of KAS.</param>
        /// <param name="iutResult">the IUT"s result of the key agreement.</param>
        protected abstract void SetTestCaseInformationFromKasResult(
            TTestGroup group,
            TTestCase testCase,
            IKas<
                TKasDsaAlgoAttributes,
                OtherPartySharedInformation<
                    TDomainParameters,
                    TKeyPair
                >,
                TDomainParameters,
                TKeyPair
            > serverKas,
            IKas<
                TKasDsaAlgoAttributes,
                OtherPartySharedInformation<
                    TDomainParameters,
                    TKeyPair
                >,
                TDomainParameters,
                TKeyPair
            > iutKas,
            KasResult iutResult
        );
    }
}