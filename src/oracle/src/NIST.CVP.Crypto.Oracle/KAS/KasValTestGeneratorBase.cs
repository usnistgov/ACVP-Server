using System;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Fakes;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Oracle.KAS
{
    public abstract class KasValTestGeneratorBase<
        TKasValParameters, TKasValResult, 
        TKasDsaAlgoAttributes, TDomainParameters, TKeyPair> : 
        IKasValTestGenerator<TKasValParameters, TKasValResult>
        where TKasValParameters : KasValParametersBase
        where TKasValResult : KasValResultBase, new()
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        protected readonly IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair 
        > _kasBuilder;
        protected readonly ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair 
        > _schemeBuilder;
        protected readonly EntropyProviderFactory _entropyProviderFactory;
        protected readonly MacParametersBuilder _macParametersBuilder;

        protected KasValTestGeneratorBase(
            IKasBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair 
            > kasBuilder, 
            ISchemeBuilder<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair 
            > schemeBuilder)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = new EntropyProviderFactory();
            _macParametersBuilder = new MacParametersBuilder();
        }

        public TKasValResult GetTest(TKasValParameters param)
        {
            var result = new TKasValResult()
            {
                TestPassed = true
            };

            var macParameters = new MacParametersBuilder()
                .WithKeyAgreementMacType(param.MacType)
                .WithMacLength(param.MacLen)
                .WithNonce(new EntropyProvider(new Random800_90()).GetEntropy(param.AesCcmNonceLen))
                .Build();

            if (param.AesCcmNonceLen != 0)
            {
                result.NonceAesCcm = macParameters.CcmNonce.GetDeepCopy();
            }

            var iutKeyConfirmationRole = param.IutKeyConfirmationRole;
            var serverKeyConfirmationRole = param.ServerKeyConfirmationRole;

            // Handles Failures due to changed z, dkm, macData
            var shaFactory = new ShaFactory();
            IKdfFactory kdfFactory = new KdfFactory(shaFactory);
            if (param.KasValTestDisposition == KasValTestDisposition.FailChangedZ)
            {
                result.TestPassed = false;
                kdfFactory = new FakeKdfFactory_BadZ(kdfFactory);
            }
            if (param.KasValTestDisposition == KasValTestDisposition.FailChangedDkm)
            {
                result.TestPassed = false;
                kdfFactory = new FakeKdfFactory_BadDkm(kdfFactory);
            }
            INoKeyConfirmationFactory noKeyConfirmationFactory = new NoKeyConfirmationFactory();
            if (param.KasValTestDisposition == KasValTestDisposition.FailChangedMacData)
            {
                result.TestPassed = false;
                noKeyConfirmationFactory = new FakeNoKeyConfirmationFactory_BadMacData(noKeyConfirmationFactory);
            }
            IKeyConfirmationFactory keyConfirmationFactory = new KeyConfirmationFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new ShaFactory()));
            if (param.KasValTestDisposition == KasValTestDisposition.FailChangedMacData)
            {
                result.TestPassed = false;
                keyConfirmationFactory = new FakeKeyConfirmationFactory_BadMacData(keyConfirmationFactory);
            }

            var uParty = GetKasInstance(
                KeyAgreementRole.InitiatorPartyU,
                param.IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                    ? iutKeyConfirmationRole
                    : serverKeyConfirmationRole,
                macParameters,
                param,
                result,
                param.IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                    ? param.IdIut
                    : param.IdServer,
                kdfFactory,
                noKeyConfirmationFactory,
                keyConfirmationFactory
            );

            var vParty = GetKasInstance(
                KeyAgreementRole.ResponderPartyV,
                param.IutKeyAgreementRole == KeyAgreementRole.ResponderPartyV
                    ? iutKeyConfirmationRole
                    : serverKeyConfirmationRole,
                macParameters,
                param,
                result,
                param.IutKeyAgreementRole == KeyAgreementRole.ResponderPartyV
                    ? param.IdIut
                    : param.IdServer,
                kdfFactory,
                noKeyConfirmationFactory,
                keyConfirmationFactory
            );

            TDomainParameters domainParameters = GetDomainParameters(param);

            uParty.SetDomainParameters(domainParameters);
            vParty.SetDomainParameters(domainParameters);

            var uPartyPublic = uParty.ReturnPublicInfoThisParty();
            result.NonceNoKc = uPartyPublic.NoKeyConfirmationNonce;
            var vPartyPublic = vParty.ReturnPublicInfoThisParty();

            var serverKas = param.IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU ? vParty : uParty;
            var iutKas = param.IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU ? uParty : vParty;

            // Mangle the keys prior to running compute result, to create a "successful" result on bad keys.
            // IUT should pick up on bad private/public key information.
            MangleKeys(
                result,
                param.KasValTestDisposition,
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

            SetResultInformationFromKasProcessing(param, result, serverKas, iutKas, iutResult);

            // Change data for failures that do not require a rerun of functions
            if (param.KasValTestDisposition == KasValTestDisposition.FailChangedOi)
            {
                result.TestPassed = false;
                result.OtherInfo[0] += 2;
            }
            if (param.KasValTestDisposition == KasValTestDisposition.FailChangedTag)
            {
                if (result.Tag != null)
                {
                    result.TestPassed = false;
                    result.Tag[0] += 2;
                }
                if (result.HashZ != null)
                {
                    result.TestPassed = false;
                    result.HashZ[0] += 2;
                }
            }

            // check for successful conditions w/ constraints.
            if (param.KasValTestDisposition == KasValTestDisposition.SuccessLeadingZeroNibbleZ)
            {
                // No zero nibble in MSB
                if (result.Z[0] >= 0x10)
                {
                    // call generate again, until getting to a zero nibble MSB for Z
                    GetTest(param);
                }
            }

            // check for successful conditions w/ constraints.
            if (param.KasValTestDisposition == KasValTestDisposition.SuccessLeadingZeroNibbleDkm)
            {
                // No zero nibble in MSB
                if (result.Dkm[0] >= 0x10)
                {
                    // call generate again, until getting to a zero nibble MSB for Z
                    GetTest(param);
                }
            }

            return result;
        }

        protected abstract IKas<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair 
        > GetKasInstance(
            KeyAgreementRole partyRole,
            KeyConfirmationRole partyKcRole,
            MacParameters macParameters,
            TKasValParameters param,
            TKasValResult result,
            BitString partyId,
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory
        );

        /// <summary>
        /// Get the domain parameters from the parameter information
        /// </summary>
        /// <param name="param">Test case generation information</param>
        /// <returns></returns>
        protected abstract TDomainParameters GetDomainParameters(TKasValParameters param);

        /// <summary>
        /// Mangles partyU/partyV ephemeral/static keys dependant on the <see cref="intendedDisposition"/>
        /// </summary>
        /// <param name="result">The <see cref="TKasValResult"/></param>
        /// <param name="intendedDisposition">The intended outcome of the test</param>
        /// <param name="serverKas">THe server party's KAS instance</param>
        /// <param name="iutKas">THe IUT party's KAS instance</param>
        protected abstract void MangleKeys(
            TKasValResult result,
            KasValTestDisposition intendedDisposition,
            IKas<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
            > serverKas,
            IKas<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
            > iutKas
        );

        /// <summary>
        /// Sets the KAS instance's generated information on the test case.
        /// </summary>
        /// <param name="param">The test parameters.</param>
        /// <param name="result">The test result.</param>
        /// <param name="serverKas">The server's instance of KAS.</param>
        /// <param name="iutKas">The IUT's instance of KAS.</param>
        /// <param name="iutResult">the IUT"s result of the key agreement.</param>
        protected abstract void SetResultInformationFromKasProcessing(
            TKasValParameters param,
            TKasValResult result,
            IKas<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
            > serverKas,
            IKas<TKasDsaAlgoAttributes, OtherPartySharedInformation<TDomainParameters, TKeyPair>, TDomainParameters, TKeyPair
                > iutKas,
            KasResult iutResult
        );
    }
}
