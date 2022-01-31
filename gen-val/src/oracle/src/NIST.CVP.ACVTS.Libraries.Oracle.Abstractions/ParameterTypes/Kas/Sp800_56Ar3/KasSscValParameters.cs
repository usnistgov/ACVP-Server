using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasSscValParameters
    {
        public KasSscTestCaseExpectation Disposition { get; set; }
        public KasScheme KasScheme { get; set; }
        public KasAlgorithm KasAlgorithm { get; set; }
        public KasDpGeneration KasDpGeneration { get; set; }
        public HashFunctions HashFunctionZ { get; set; }

        public IDsaDomainParameters DomainParameters { get; set; }
        public IDsaKeyPair ServerEphemeralKey { get; set; }
        public IDsaKeyPair ServerStaticKey { get; set; }
        public IDsaKeyPair IutEphemeralKey { get; set; }
        public IDsaKeyPair IutStaticKey { get; set; }
        public SchemeKeyNonceGenRequirement IutGenerationRequirements { get; set; }
        public SchemeKeyNonceGenRequirement ServerGenerationRequirements { get; set; }
    }
}
