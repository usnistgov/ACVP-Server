using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
	public class TlsKdfv13Parameters
	{
		public HashFunctions HashAlg { get; set; }
		public int RandomLength { get; set; }
	}
}