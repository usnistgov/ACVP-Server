using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3
{
    public abstract class KasParametersBase
    {
        public KasScheme KasScheme { get; set; }
        public KasAlgorithm KasAlgorithm { get; set; }

        public IDsaDomainParameters DomainParameters { get; set; }
        public KasDpGeneration KasDpGeneration { get; set; }

        public BitString PartyIdServer { get; set; }

        public SchemeKeyNonceGenRequirement IutGenerationRequirements { get; set; }
        public SchemeKeyNonceGenRequirement ServerGenerationRequirements { get; set; }
    }
}
