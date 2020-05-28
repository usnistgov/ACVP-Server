using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3
{
	public class KasSscValParameters
	{
		public KasSscTestCaseExpectation Disposition { get; set; }
		public KasScheme KasScheme { get; set; }
		public KasAlgorithm KasAlgorithm { get; set; }
        
		public IDsaDomainParameters DomainParameters { get; set; }
		public SchemeKeyNonceGenRequirement IutGenerationRequirements { get; set; }
		public SchemeKeyNonceGenRequirement ServerGenerationRequirements { get; set; }
	}
}