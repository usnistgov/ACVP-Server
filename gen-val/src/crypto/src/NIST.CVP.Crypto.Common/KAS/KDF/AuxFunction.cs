using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
	public class AuxFunction
	{
		/// <summary>
		/// The Hash or Mac function name
		/// </summary>
		public KasKdfOneStepAuxFunction AuxFunctionName { get; set; }
		/// <summary>
		/// The salting methods used for the KDF (hashes do not require salts, MACs do)
		/// </summary>
		public MacSaltMethod[] MacSaltMethods { get; set; }
	}
}