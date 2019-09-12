using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KasAftParametersIfc
    {
        public bool IsSample { get; set; }
        
        public IfcScheme Scheme { get; set; }
        
        public KasMode KasMode { get; set; }
        
        public BitString IutPartyId { get; set; }
        
        public KeyAgreementRole IutKeyAgreementRole { get; set; }
        
        public KeyAgreementRole ServerKeyAgreementRole => IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU
            ? KeyAgreementRole.ResponderPartyV
            : KeyAgreementRole.InitiatorPartyU;
        
        public KeyConfirmationRole IutKeyConfirmationRole { get; set; }

        public BitString ServerPartyId { get; set; }
        
        public KeyConfirmationRole ServerKeyConfirmationRole
        {
            get
            {
                if (IutKeyConfirmationRole == KeyConfirmationRole.None)
                {
                    return KeyConfirmationRole.None;
                }

                return IutKeyConfirmationRole == KeyConfirmationRole.Provider
                    ? KeyConfirmationRole.Recipient
                    : KeyConfirmationRole.Provider;
            }
        }
        
        public KeyConfirmationDirection KeyConfirmationDirection { get; set; }
        
        public IfcKeyGenerationMethod KeyGenerationMethod { get; set; }
        
        public int Modulo { get; set; }
        
        public BigInteger PublicExponent { get; set; }
        
        public int L { get; set; }
        
        public KeyPair IutKey { get; set; }
        
        public IKasKdfConfiguration KdfConfiguration { get; set; }
        public KtsConfiguration KtsConfiguration { get; set; }
        public MacConfiguration MacConfiguration { get; set; }
    }
}