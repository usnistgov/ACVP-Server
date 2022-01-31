using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasSscAftDeferredParameters
    {
        public KasScheme KasScheme { get; set; }
        public HashFunctions HashFunctionZ { get; set; }

        public IDsaDomainParameters DomainParameters { get; set; }

        public SchemeKeyNonceGenRequirement IutGenerationRequirements { get; set; }
        public SchemeKeyNonceGenRequirement ServerGenerationRequirements { get; set; }

        public IDsaKeyPair EphemeralKeyServer { get; set; }
        public IDsaKeyPair StaticKeyServer { get; set; }

        public IDsaKeyPair EphemeralKeyIut { get; set; }
        public IDsaKeyPair StaticKeyIut { get; set; }
    }
}
