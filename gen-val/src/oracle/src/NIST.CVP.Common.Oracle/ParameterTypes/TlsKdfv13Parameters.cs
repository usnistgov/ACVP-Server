using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
	public class TlsKdfv13Parameters
	{
		public HashFunctions HashAlg { get; set; }
		public TlsModes1_3 RunningMode { get; set; }
		public int RandomLength { get; set; }
	}
}