using System;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class SchemeEccEphemeralUnified : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccEphemeralUnified(
            IDsaEcc dsa, 
            IEccCurveFactory eccCurveFactory, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            INoKeyConfirmationFactory noKeyConfirmationFactory, 
            IOtherInfoFactory<
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > otherInfoFactory, 
            IEntropyProvider entropyProvider, 
            SchemeParametersBase<
                EccParameterSet, 
                EccScheme
            > schemeParameters, 
            KdfParameters kdfParameters, 
            MacParameters macParameters,
            IDiffieHellman<EccDomainParameters, EccKeyPair> diffieHellman
        ) 
            : base(
                  dsa, 
                  eccCurveFactory, 
                  kdfFactory, 
                  keyConfirmationFactory, 
                  noKeyConfirmationFactory, 
                  otherInfoFactory, 
                  entropyProvider, 
                  schemeParameters, 
                  kdfParameters, 
                  macParameters
              )
        {
            _diffieHellman = diffieHellman;

            if (SchemeParameters.Scheme != EccScheme.EphemeralUnified)
            {
                throw new ArgumentException(nameof(SchemeParameters.Scheme));
            }

            if (SchemeParameters.KasMode == KasMode.KdfKc)
            {
                throw new ArgumentException($"{SchemeParameters.KasMode} not possible with {SchemeParameters.Scheme}");
            }
        }

        protected override void GenerateKasKeyNonceInformation()
        {
            if (DomainParameters == null)
            {
                GenerateDomainParameters();
            }

            EphemeralKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;

            // when party U and KdfNoKc, a NoKeyConfirmationNonce is needed.
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                && SchemeParameters.KasMode == KasMode.KdfNoKc)
            {
                NoKeyConfirmationNonce = EntropyProvider.GetEntropy(128);
            }
        }

        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<EccDomainParameters, EccKeyPair> otherPartyInformation)
        {
            return _diffieHellman.GenerateSharedSecretZ(
                DomainParameters,
                EphemeralKeyPair,
                otherPartyInformation.EphemeralPublicKey
            ).SharedSecretZ;
        }
    }
}