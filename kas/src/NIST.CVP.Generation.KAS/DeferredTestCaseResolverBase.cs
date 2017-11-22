using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS
{
    public abstract class DeferredTestCaseResolverBase<
        TTestGroup, 
        TTestCase, 
        TKasDsaAlgoAttributes, 
        TDomainParameters, 
        TKeyPair,
        TScheme
    > 
        : IDeferredTestCaseResolver<TTestGroup, TTestCase, KasResult>
        where TTestGroup : TestGroupBase<TKasDsaAlgoAttributes>
        where TTestCase : TestCaseBase
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
        where TScheme : struct, IComparable
    {
        protected readonly IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> _kasBuilder;
        protected readonly IMacParametersBuilder _macParametersBuilder;
        protected readonly ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> _schemeBuilder;
        protected readonly IEntropyProviderFactory _entropyProviderFactory;

        protected DeferredTestCaseResolverBase(
            IKasBuilder<
                TKasDsaAlgoAttributes,
                OtherPartySharedInformation<
                    TDomainParameters,
                    TKeyPair
                >,
                TDomainParameters,
                TKeyPair
            > kasBuilder,
            IMacParametersBuilder macParametersBuilder,
            ISchemeBuilder<
                TKasDsaAlgoAttributes,
                OtherPartySharedInformation<
                    TDomainParameters,
                    TKeyPair
                >,
                TDomainParameters,
                TKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory
        )
        {
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        /// <inheritdoc />
        public KasResult CompleteDeferredCrypto(TTestGroup testGroup, TTestCase serverTestCase,
            TTestCase iutTestCase)
        {
            KeyAgreementRole serverRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroup.KasRole);
            KeyConfirmationRole serverKcRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(testGroup.KcRole);

            var serverKeyRequirements = GetServerNonceKeyGenRequirements(
                testGroup, 
                serverRole, 
                serverKcRole
            );

            var domainParameters = GetDomainParameters(testGroup);

            // Gets the IUTs information to use as the "other party" in a server KAS negotiation.
            var iutSharedInformation = GetIutSharedInformation(
                testGroup, 
                serverTestCase,
                iutTestCase, 
                domainParameters
            );

            var macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(testGroup.MacType)
                .WithMacLength(testGroup.MacLen)
                .WithNonce(iutTestCase.NonceAesCcm ?? serverTestCase.NonceAesCcm)
                .Build();

            // KdfNoKc mode requires the use of a Nonce
            if (testGroup.KasMode == KasMode.KdfNoKc)
            {
                var entropyProvider = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProvider.AddEntropy(serverTestCase.NonceNoKc ?? iutTestCase.NonceNoKc);

                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // If the server has a requirement of generating an ephemeral nonce, 
            // inject it into the entropy provider
            if (serverKeyRequirements.GeneratesEphemeralNonce)
            {
                var entropyProvider = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProvider.AddEntropy(serverTestCase.EphemeralNonceServer);

                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // Gets the KAS instance specific to the options of the extended class
            var serverKas = GetServerKas(
                serverKeyRequirements, 
                serverRole, 
                serverKcRole, 
                macParameters, 
                testGroup, 
                iutTestCase
            );

            serverKas.SetDomainParameters(domainParameters);
            serverKas.ReturnPublicInfoThisParty();

            // Update the KAS instance with the needed key pairs
            UpdateKasInstanceWithTestCaseInformation(serverKas, serverKeyRequirements, serverTestCase);

            var serverResult = serverKas.ComputeResult(iutSharedInformation);
            return serverResult;
        }

        /// <summary>
        /// Gets the nonce and key generation requirements for the server, as per the options provided.
        /// </summary>
        /// <param name="testGroup">The test group information.</param>
        /// <param name="serverKeyAgreementRole">The server party's key agreement role.</param>
        /// <param name="serverKeyConfirmationRole">The server party's key confirmation role.</param>
        /// <returns></returns>
        protected abstract SchemeKeyNonceGenRequirement<TScheme> GetServerNonceKeyGenRequirements(
            TTestGroup testGroup, 
            KeyAgreementRole serverKeyAgreementRole, 
            KeyConfirmationRole serverKeyConfirmationRole
        );

        /// <summary>
        /// Get the domain parameters as described by the test group.
        /// </summary>
        /// <param name="testGroup">The test group to retrieve the domain parameters from.</param>
        /// <returns></returns>
        protected abstract TDomainParameters GetDomainParameters(TTestGroup testGroup);

        /// <summary>
        /// retrieve the "IUT/other party information" to be passed into the server's KAS instance 
        /// for completing the negotiation.
        /// </summary>
        /// <param name="testGroup">The test group.</param>
        /// <param name="serverTestCase">The server's pre-generated test case information.</param>
        /// <param name="iutTestCase">The IUT's provided information.</param>
        /// <param name="domainParameters">The domain parameters.</param>
        /// <returns></returns>
        protected abstract OtherPartySharedInformation<
            TDomainParameters, 
            TKeyPair
        > GetIutSharedInformation(
            TTestGroup testGroup, 
            TTestCase serverTestCase,
            TTestCase iutTestCase, 
            TDomainParameters domainParameters
        );

        /// <summary>
        /// Retrieves the KAS instance to use in the key agreement.
        /// </summary>
        /// <param name="serverKeyRequirements">The server's key/nonce generation requirements.</param>
        /// <param name="serverRole">The server's key agreement role.</param>
        /// <param name="serverKcRole">The server's key confirmation role.</param>
        /// <param name="macParameters">The mac parameters to utilize.</param>
        /// <param name="testGroup">The test group.</param>
        /// <param name="iutTestCase">The IUT's test case information.</param>
        /// <returns></returns>
        protected abstract IKas<
            TKasDsaAlgoAttributes, 
            OtherPartySharedInformation<
                TDomainParameters, 
                TKeyPair
            >, 
            TDomainParameters,
            TKeyPair
        > GetServerKas(
            SchemeKeyNonceGenRequirement<TScheme> serverKeyRequirements, 
            KeyAgreementRole serverRole, 
            KeyConfirmationRole serverKcRole, 
            MacParameters macParameters,
            TTestGroup testGroup, 
            TTestCase iutTestCase
        );
        
        /// <summary>
        /// Updates the KAS instance with the appropriate server keypair information.
        /// </summary>
        /// <param name="serverKas">The server's KAS instance.</param>
        /// <param name="serverKeyRequirements">The server's key/nonce gen requirements.</param>
        /// <param name="serverTestCase">The server's test case.</param>
        protected abstract void UpdateKasInstanceWithTestCaseInformation(
            IKas<
                TKasDsaAlgoAttributes, 
                OtherPartySharedInformation<
                    TDomainParameters, 
                    TKeyPair
                >, 
                TDomainParameters, 
                TKeyPair
            > serverKas,
            SchemeKeyNonceGenRequirement<TScheme> serverKeyRequirements, 
            TTestCase serverTestCase
        );
    }
}