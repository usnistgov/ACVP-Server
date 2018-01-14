using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS
{
    public abstract class TestCaseGeneratorAftBase<
        TTestGroup, 
        TTestCase, 
        TKasDsaAlgoAttributes, 
        TDomainParameters, 
        TKeyPair,
        TScheme
    > 
        : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TKasDsaAlgoAttributes>
        where TTestCase : TestCaseBase, new()
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
        where TScheme : struct, IComparable
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
        protected readonly IEntropyProviderFactory _entropyProviderFactory;
        protected readonly IMacParametersBuilder _macParametersBuilder;

        protected TestCaseGeneratorAftBase(
            IKasBuilder<
                TKasDsaAlgoAttributes, 
                OtherPartySharedInformation<
                    TDomainParameters, 
                    TKeyPair
                >, 
                TDomainParameters, 
                TKeyPair
            > kasBuilder,
            ISchemeBuilder<
                TKasDsaAlgoAttributes, 
                OtherPartySharedInformation<
                    TDomainParameters, 
                    TKeyPair
                >, 
                TDomainParameters, 
                TKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder
        )
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
            _macParametersBuilder = macParametersBuilder;
        }

        public int NumberOfTestCasesToGenerate => 10;
        public TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample)
        {
            TTestCase testCase = new TTestCase()
            {
                Deferred = true
            };

            KeyAgreementRole serverRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(group.KasRole);
            KeyConfirmationRole serverKcRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(group.KcRole);

            var serverKeyNonceRequirements = GetPartyNonceKeyGenRequirements(
                group,
                serverRole,
                serverKcRole
            );

            var entropyProvider = _entropyProviderFactory
                .GetEntropyProvider(EntropyProviderTypes.Testable);

            // If the server has a requirement of generating an DKM nonce, 
            // inject it into the entropy provider
            if (serverKeyNonceRequirements.GeneratesDkmNonce)
            {
                var dkmNonceLength = GetDkmLengthRequirement(group);

                testCase.DkmNonceServer = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(dkmNonceLength);
                entropyProvider.AddEntropy(testCase.DkmNonceServer.GetDeepCopy());
                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // Set up entropy injection when server generates an ephemeral nonce
            if (serverKeyNonceRequirements.GeneratesEphemeralNonce)
            {
                var ephemeralNonceLength = GetEphemeralLengthRequirement(group);

                testCase.EphemeralNonceServer = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(ephemeralNonceLength);
                entropyProvider.AddEntropy(testCase.EphemeralNonceServer.GetDeepCopy());
                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // a nonce is used for KdfNoKc, set up entropy injection
            if (group.KasMode == KasMode.KdfNoKc)
            {
                testCase.NonceNoKc = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(128);

                entropyProvider.AddEntropy(testCase.NonceNoKc.GetDeepCopy());
                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            BitString aesCcmNonce = null;
            if ((serverRole == KeyAgreementRole.InitiatorPartyU && group.MacType == KeyAgreementMacType.AesCcm) || isSample)
            {
                aesCcmNonce = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(group.AesCcmNonceLen);

                testCase.NonceAesCcm = aesCcmNonce;
            }

            MacParameters macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(group.MacType)
                .WithMacLength(group.MacLen)
                .WithNonce(aesCcmNonce)
                .Build();

            var serverKas = GetKasInstance(
                serverKeyNonceRequirements, 
                serverRole, 
                serverKcRole, 
                macParameters, 
                group,
                testCase, 
                SpecificationMapping.ServerId
            );

            TDomainParameters domainParametrs = GetGroupDomainParameters(group);
            serverKas.SetDomainParameters(domainParametrs);

            var serverPublicInfo = serverKas.ReturnPublicInfoThisParty();

            SetTestCaseInformationFromKasResult(group, testCase, serverKas, null, null);

            // For sample, we need to generate everything up front so that something's available
            // in the answer files
            if (isSample)
            {
                testCase.Deferred = false;

                _schemeBuilder.WithEntropyProvider(
                    _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                );

                var iutKeyNonceRequirements = GetPartyNonceKeyGenRequirements(
                    group,
                    group.KasRole,
                    group.KcRole
                );

                var entropyProviderSample = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);

                if (iutKeyNonceRequirements.GeneratesDkmNonce)
                {
                    var dkmNonceLength = GetDkmLengthRequirement(group);

                    testCase.DkmNonceIut = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                        .GetEntropy(dkmNonceLength);

                    entropyProviderSample.AddEntropy(testCase.DkmNonceIut.GetDeepCopy());
                    _schemeBuilder.WithEntropyProvider(entropyProviderSample);
                }

                if (iutKeyNonceRequirements.GeneratesEphemeralNonce)
                {
                    var ephemeralNonceLength = GetEphemeralLengthRequirement(group);
                    
                    testCase.EphemeralNonceIut = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                        .GetEntropy(ephemeralNonceLength);

                    entropyProviderSample.AddEntropy(testCase.EphemeralNonceIut.GetDeepCopy());
                    _schemeBuilder.WithEntropyProvider(entropyProviderSample);
                }

                if (group.KasMode == KasMode.KdfNoKc)
                {
                    entropyProviderSample.AddEntropy(testCase.NonceNoKc.GetDeepCopy());
                    _schemeBuilder.WithEntropyProvider(entropyProviderSample);
                }

                if (group.AesCcmNonceLen != 0)
                {
                    testCase.NonceAesCcm = macParameters.CcmNonce.GetDeepCopy();
                }

                testCase.IdIut = SpecificationMapping.IutId;
                testCase.IdIutLen = testCase.IdIut.BitLength;

                var iutKas = GetKasInstance(
                    iutKeyNonceRequirements, 
                    group.KasRole, 
                    group.KcRole, 
                    macParameters, 
                    group,
                    testCase, 
                    SpecificationMapping.IutId
                );

                var result = iutKas.ComputeResult(serverPublicInfo);

                SetTestCaseInformationFromKasResult(group, testCase, serverKas, iutKas, result);
            }

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse(testCase);
        }

        /// <summary>
        /// Gets the nonce and key generation requirements for the party, as per the options provided.
        /// </summary>
        /// <param name="testGroup">The test group information.</param>
        /// <param name="partyKeyAgreementRole">The party's key agreement role.</param>
        /// <param name="partyKeyConfirmationRole">The party's key confirmation role.</param>
        /// <returns></returns>
        protected abstract SchemeKeyNonceGenRequirement<TScheme> GetPartyNonceKeyGenRequirements(
            TTestGroup testGroup,
            KeyAgreementRole partyKeyAgreementRole,
            KeyConfirmationRole partyKeyConfirmationRole
        );

        /// <summary>
        /// Gets the length requirement for the ephemeral key/nonce.
        /// </summary>
        /// <param name="testGroup">The test group.</param>
        /// <returns></returns>
        protected abstract int GetEphemeralLengthRequirement(TTestGroup testGroup);

        /// <summary>
        /// Gets the length requirement for the dkm nonce.
        /// </summary>
        /// <param name="testGroup">The test group</param>
        /// <returns></returns>
        protected abstract int GetDkmLengthRequirement(TTestGroup testGroup);

        /// <summary>
        /// Gets the KAS instance based on the provided parameters.
        /// </summary>
        /// <param name="partyKeyNonceRequirements">The party's key/nonce gen requirements.</param>
        /// <param name="partyRole">The party's role.</param>
        /// <param name="partyKcRole">The party's key confirmation role.</param>
        /// <param name="macParameters">The MAC parameters.</param>
        /// <param name="group">The test group.</param>
        /// <param name="testCase">The test case.</param>
        /// <param name="partyId">The party's identifier.</param>
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
            SchemeKeyNonceGenRequirement<TScheme> partyKeyNonceRequirements,
            KeyAgreementRole partyRole,
            KeyConfirmationRole partyKcRole,
            MacParameters macParameters,
            TTestGroup @group,
            TTestCase testCase,
            BitString partyId
        );

        /// <summary>
        /// Gets the domain parameters from the group.
        /// </summary>
        /// <param name="testGroup">The test group to retrieve the domain parameters from.</param>
        /// <returns></returns>
        protected abstract TDomainParameters GetGroupDomainParameters(TTestGroup testGroup);

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