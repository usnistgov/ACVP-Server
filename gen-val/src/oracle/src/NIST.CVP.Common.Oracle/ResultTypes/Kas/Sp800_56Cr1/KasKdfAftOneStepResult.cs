using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Cr1
{
	public class KasKdfAftOneStepResult
	{
		public KdfParameterOneStep KdfInputs { get; set; }
		public PartyFixedInfo FixedInfoPartyU { get; set; }
		public PartyFixedInfo FixedInfoPartyV { get; set; }
		public BitString DerivedKeyingMaterial { get; set; }
	}
}