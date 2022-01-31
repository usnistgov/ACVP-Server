using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1
{
    public abstract class KasAftDeferredParametersBase
    {
        public KasMode KasMode { get; set; }

        public KeyAgreementRole IutKeyAgreementRole { get; set; }

        public KeyAgreementRole ServerKeyAgreementRole => IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU
            ? KeyAgreementRole.ResponderPartyV
            : KeyAgreementRole.InitiatorPartyU;

        public KeyConfirmationRole IutKeyConfirmationRole { get; set; }

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

        public HashFunction HashFunction { get; set; }

        public KeyAgreementMacType MacType { get; set; }

        public int KeyLen { get; set; }

        public int MacLen { get; set; }

        public BitString IdServer { get; set; } = new BitString("434156536964");

        public BitString IdIut { get; set; }

        public string OiPattern { get; set; }

        public BitString DkmNonceServer { get; set; }

        public BitString EphemeralNonceServer { get; set; }


        public BitString DkmNonceIut { get; set; }

        public BitString EphemeralNonceIut { get; set; }


        public int OiLen { get; set; }

        public BitString OtherInfo { get; set; }

        public BitString NonceNoKc { get; set; }

        public BitString NonceAesCcm { get; set; }
    }
}
