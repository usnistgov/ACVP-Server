using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1
{
    public abstract class KasValParametersBase
    {
        public KasMode KasMode { get; set; }

        public KasValTestDisposition KasValTestDisposition { get; set; }

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

        public int AesCcmNonceLen { get; set; }

        public int MacLen { get; set; }
        
        public BitString IdServer { get; set; } = new BitString("434156536964");

        public BitString IdIut { get; set; } = new BitString("a1b2c3d4e5");

        public string OiPattern { get; set; }
    }
}
