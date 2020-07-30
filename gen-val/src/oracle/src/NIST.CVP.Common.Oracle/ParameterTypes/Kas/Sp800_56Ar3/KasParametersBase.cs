using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3
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