using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme.Ecc
{
    public class SchemeEccEphemeralUnified : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccEphemeralUnified(
            IDsaEcc dsa, 
            IEccCurveFactory eccCurveFactory, 
            IKdfOneStepFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            INoKeyConfirmationFactory noKeyConfirmationFactory, 
            IOtherInfoFactory otherInfoFactory, 
            IEntropyProvider entropyProvider, 
            SchemeParametersBase<KasDsaAlgoAttributesEcc> schemeParameters, 
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

            if (SchemeParameters.KasAlgoAttributes.Scheme != EccScheme.EphemeralUnified)
            {
                throw new ArgumentException(nameof(SchemeParameters.KasAlgoAttributes.Scheme));
            }

            if (SchemeParameters.KasMode == KasMode.KdfKc)
            {
                throw new ArgumentException($"{SchemeParameters.KasMode} not possible with {SchemeParameters.KasAlgoAttributes.Scheme}");
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