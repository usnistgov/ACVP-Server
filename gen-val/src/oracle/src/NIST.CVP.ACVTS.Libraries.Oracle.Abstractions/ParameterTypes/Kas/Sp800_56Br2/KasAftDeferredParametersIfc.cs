using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2
{
    public class KasAftDeferredParametersIfc
    {
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

        public int Modulo { get; set; }

        public int L { get; set; }

        public KeyPair IutKey { get; set; }
        public KeyPair ServerKey { get; set; }

        public BitString IutC { get; set; }
        public BitString ServerC { get; set; }

        public BitString ServerZ { get; set; }
        public BitString IutZ { get; set; }

        public BitString IutNonce { get; set; }
        public BitString ServerNonce { get; set; }

        public BitString IutK { get; set; }
        public BitString ServerK { get; set; }

        public IKdfParameter KdfParameter { get; set; }
        public KtsParameter KtsParameter { get; set; }
        public MacParameters MacParameter { get; set; }
    }
}
