using System.Collections.Generic;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.Fakes;
using NIST.CVP.Generation.KAS.Helpers;
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
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>, new()
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
        protected List<KasValTestDisposition> _dispositionList;

        protected TestCaseGeneratorValBase(
            IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> kasBuilder,
            ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> schemeBuilder,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            List<KasValTestDisposition> dispositionList
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
            _dispositionList = dispositionList;
        }

        public int NumberOfTestCasesToGenerate => 25;
        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, bool isSample)
        {
            var testCase = new TTestCase()
            {
                TestCaseDisposition = TestCaseDispositionHelper.GetTestCaseIntention(_dispositionList),
                TestPassed = true
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, TTestCase testCase)
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
            if (testCase.TestCaseDisposition == KasValTestDisposition.FailChangedZ)
            {
                testCase.TestPassed = false;
                kdfFactory = new FakeKdfFactory_BadZ(_kdfFactory);
            }
            if (testCase.TestCaseDisposition == KasValTestDisposition.FailChangedDkm)
            {
                testCase.TestPassed = false;
                kdfFactory = new FakeKdfFactory_BadDkm(_kdfFactory);
            }
            INoKeyConfirmationFactory noKeyConfirmationFactory = _noKeyConfirmationFactory;
            if (testCase.TestCaseDisposition == KasValTestDisposition.FailChangedMacData)
            {
                testCase.TestPassed = false;
                noKeyConfirmationFactory = new FakeNoKeyConfirmationFactory_BadMacData(_noKeyConfirmationFactory);
            }
            IKeyConfirmationFactory keyConfirmationFactory = _keyConfirmationFactory;
            if (testCase.TestCaseDisposition == KasValTestDisposition.FailChangedMacData)
            {
                testCase.TestPassed = false;
                keyConfirmationFactory = new FakeKeyConfirmationFactory_BadMacData(_keyConfirmationFactory);
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
                testCase.TestCaseDisposition,
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
            if (testCase.TestCaseDisposition == KasValTestDisposition.FailChangedOi)
            {
                testCase.TestPassed = false;
                testCase.OtherInfo[0] += 2;
            }
            if (testCase.TestCaseDisposition == KasValTestDisposition.FailChangedTag)
            {
                if (testCase.Tag != null)
                {
                    testCase.TestPassed = false;
                    testCase.Tag[0] += 2;
                }
                if (testCase.HashZ != null)
                {
                    testCase.TestPassed = false;
                    testCase.HashZ[0] += 2;
                }
            }

            // check for successful conditions w/ constraints.
            if (testCase.TestCaseDisposition == KasValTestDisposition.SuccessLeadingZeroNibbleZ)
            {
                // No zero nibble in MSB
                if (testCase.Z[0] >= 0x10)
                {
                    // call generate again, until getting to a zero nibble MSB for Z
                    Generate(group, testCase);
                }
            }

            // check for successful conditions w/ constraints.
            if (testCase.TestCaseDisposition == KasValTestDisposition.SuccessLeadingZeroNibbleDkm)
            {
                // No zero nibble in MSB
                if (testCase.Dkm[0] >= 0x10)
                {
                    // call generate again, until getting to a zero nibble MSB for Z
                    Generate(group, testCase);
                }
            }

            return new TestCaseGenerateResponse<TTestGroup, TTestCase>(testCase);
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
            KasValTestDisposition intendedDisposition,
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