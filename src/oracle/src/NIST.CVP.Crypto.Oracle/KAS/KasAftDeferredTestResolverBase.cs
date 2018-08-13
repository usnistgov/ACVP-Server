using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Oracle.KAS
{
    public abstract class KasAftDeferredTestResolverBase<
            TKasAftDeferredParameters, TKasAftDeferredResult, 
            TKasDsaAlgoAttributes, TDomainParameters, TKeyPair, TScheme>
        : IKasAftDeferredTestResolver<TKasAftDeferredParameters, TKasAftDeferredResult>
        where TKasAftDeferredParameters : KasAftDeferredParametersBase
        where TKasAftDeferredResult : KasAftDeferredResult, new()
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
        where TScheme : struct, IComparable
    {

        protected readonly IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
        > _kasBuilder;
        protected readonly IMacParametersBuilder _macParametersBuilder;
        protected readonly ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
        > _schemeBuilder;
        protected readonly IEntropyProviderFactory _entropyProviderFactory;

        protected KasAftDeferredTestResolverBase(
            IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> kasBuilder,
            IMacParametersBuilder macParametersBuilder,
            ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair> schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory
        )
        {
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public TKasAftDeferredResult CompleteTest(TKasAftDeferredParameters param)
        {
            KeyAgreementRole serverRole = param.ServerKeyAgreementRole;
            KeyConfirmationRole serverKcRole = param.ServerKeyConfirmationRole;

            var serverKeyRequirements = GetServerNonceKeyGenRequirements(
                param, 
                serverRole, 
                serverKcRole
            );

            var domainParameters = GetDomainParameters(param);

            // Gets the IUTs information to use as the "other party" in a server KAS negotiation.
            var iutSharedInformation = GetIutSharedInformation(
                param, 
                domainParameters
            );

            var macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(param.MacType)
                .WithMacLength(param.MacLen)
                .WithNonce(param.NonceAesCcm)
                .Build();

            var entropyProvider = _entropyProviderFactory
                .GetEntropyProvider(EntropyProviderTypes.Testable);

            // If the server has a requirement of generating an DKM nonce, 
            // inject it into the entropy provider
            if (serverKeyRequirements.GeneratesDkmNonce)
            {
                entropyProvider.AddEntropy(param.DkmNonceServer);

                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // If the server has a requirement of generating an ephemeral nonce, 
            // inject it into the entropy provider
            if (serverKeyRequirements.GeneratesEphemeralNonce)
            {
                entropyProvider.AddEntropy(param.EphemeralNonceServer);

                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // KdfNoKc mode requires the use of a Nonce
            if (param.KasMode == KasMode.KdfNoKc)
            {
                entropyProvider.AddEntropy(param.NonceNoKc);

                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            // Gets the KAS instance specific to the options of the extended class
            var serverKas = GetServerKas(
                serverKeyRequirements, 
                serverRole, 
                serverKcRole, 
                macParameters, 
                param
            );

            serverKas.SetDomainParameters(domainParameters);
            serverKas.ReturnPublicInfoThisParty();

            // Update the KAS instance with the needed key pairs
            UpdateKasInstanceWithTestCaseInformation(serverKas, serverKeyRequirements, param);

            var serverResult = serverKas.ComputeResult(iutSharedInformation);
            return new TKasAftDeferredResult()
            {
                Result = serverResult
            };
        }

        /// <summary>
        /// Gets the nonce and key generation requirements for the server, as per the options provided.
        /// </summary>
        /// <param name="param">The test parameter information.</param>
        /// <param name="serverKeyAgreementRole">The server party's key agreement role.</param>
        /// <param name="serverKeyConfirmationRole">The server party's key confirmation role.</param>
        /// <returns></returns>
        protected abstract SchemeKeyNonceGenRequirement<TScheme> GetServerNonceKeyGenRequirements(
            TKasAftDeferredParameters param, 
            KeyAgreementRole serverKeyAgreementRole, 
            KeyConfirmationRole serverKeyConfirmationRole
        );

        /// <summary>
        /// Get the domain parameters as described by the test group.
        /// </summary>
        /// <param name="param">The test parameter to retrieve the domain parameters from.</param>
        /// <returns></returns>
        protected abstract TDomainParameters GetDomainParameters(TKasAftDeferredParameters param);

        /// <summary>
        /// retrieve the "IUT/other party information" to be passed into the server's KAS instance 
        /// for completing the negotiation.
        /// </summary>
        /// <param name="param">The test parameters.</param>
        /// <param name="domainParameters">The domain parameters.</param>
        /// <returns></returns>
        protected abstract OtherPartySharedInformation<
            TDomainParameters, 
            TKeyPair
        > GetIutSharedInformation(
            TKasAftDeferredParameters param, 
            TDomainParameters domainParameters
        );

        /// <summary>
        /// Retrieves the KAS instance to use in the key agreement.
        /// </summary>
        /// <param name="serverKeyRequirements">The server's key/nonce generation requirements.</param>
        /// <param name="serverRole">The server's key agreement role.</param>
        /// <param name="serverKcRole">The server's key confirmation role.</param>
        /// <param name="macParameters">The mac parameters to utilize.</param>
        /// <param name="param">The test parameters.</param>
        /// <returns></returns>
        protected abstract IKas<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters,TKeyPair
        > GetServerKas(
            SchemeKeyNonceGenRequirement<TScheme> serverKeyRequirements, 
            KeyAgreementRole serverRole, 
            KeyConfirmationRole serverKcRole, 
            MacParameters macParameters,
            TKasAftDeferredParameters param
        );

        /// <summary>
        /// Updates the KAS instance with the appropriate server keypair information.
        /// </summary>
        /// <param name="serverKas">The server's KAS instance.</param>
        /// <param name="serverKeyRequirements">The server's key/nonce gen requirements.</param>
        /// <param name="param">The parameters for the test.</param>
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
            TKasAftDeferredParameters param
        );
    }
}