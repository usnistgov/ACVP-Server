using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
	public class TlsKdfv13Result
	{
		public TlsKdfV13FullResult DerivedKeyingMaterial { get; set; }

		public BitString Psk { get; set; }
		public BitString Dhe { get; set; }
		
		public BitString HelloClientRandom { get; set; }
		public BitString HelloServerRandom { get; set; }
		
		public BitString FinishClientRandom { get; set; }
		public BitString FinishServerRandom { get; set; }
	}
}