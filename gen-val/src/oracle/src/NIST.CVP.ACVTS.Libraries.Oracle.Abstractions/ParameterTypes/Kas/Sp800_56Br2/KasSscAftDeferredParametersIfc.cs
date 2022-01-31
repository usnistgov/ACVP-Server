using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2
{
    public class KasSscAftDeferredParametersIfc
    {
        public SscIfcScheme Scheme { get; set; }

        public KasMode KasMode { get; set; }

        public KeyAgreementRole IutKeyAgreementRole { get; set; }

        public KeyAgreementRole ServerKeyAgreementRole => IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU
            ? KeyAgreementRole.ResponderPartyV
            : KeyAgreementRole.InitiatorPartyU;

        public int Modulo { get; set; }

        public KeyPair IutKey { get; set; }
        public KeyPair ServerKey { get; set; }

        public BitString IutC { get; set; }
        public BitString ServerC { get; set; }

        public BitString ServerZ { get; set; }
        public BitString IutZ { get; set; }

        public HashFunctions HashFunctionZ { get; set; }
    }
}
