using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2
{
    public class KasSscAftParametersIfc
    {
        public bool IsSample { get; set; }

        public SscIfcScheme Scheme { get; set; }

        public KasMode KasMode { get; set; }

        public KeyAgreementRole IutKeyAgreementRole { get; set; }

        public KeyAgreementRole ServerKeyAgreementRole => IutKeyAgreementRole == KeyAgreementRole.InitiatorPartyU
            ? KeyAgreementRole.ResponderPartyV
            : KeyAgreementRole.InitiatorPartyU;

        public PrivateKeyModes PrivateKeyMode
        {
            get
            {
                switch (KeyGenerationMethod)
                {
                    case IfcKeyGenerationMethod.RsaKpg1_crt:
                    case IfcKeyGenerationMethod.RsaKpg2_crt:
                        return PrivateKeyModes.Crt;
                    default:
                        return PrivateKeyModes.Standard;
                }
            }
        }

        public PublicExponentModes PublicExponentMode
        {
            get
            {
                switch (KeyGenerationMethod)
                {
                    case IfcKeyGenerationMethod.RsaKpg2_basic:
                    case IfcKeyGenerationMethod.RsaKpg2_crt:
                    case IfcKeyGenerationMethod.RsaKpg2_primeFactor:
                        return PublicExponentModes.Random;
                    default:
                        return PublicExponentModes.Fixed;
                }
            }
        }

        public IfcKeyGenerationMethod KeyGenerationMethod { get; set; }

        public int Modulo { get; set; }

        public BigInteger PublicExponent { get; set; }

        public SchemeKeyNonceGenRequirement ServerGenerationRequirements { get; set; }
        public SchemeKeyNonceGenRequirement IutGenerationRequirements { get; set; }

        public KeyPair ServerKeyPair { get; set; }
        public KeyPair IutKeyPair { get; set; }
    }
}
