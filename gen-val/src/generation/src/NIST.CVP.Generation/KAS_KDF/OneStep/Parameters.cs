using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class Parameters : IParameters
	{
		public int VectorSetId { get; set; }
		public string Algorithm { get; set; }
		public string Mode { get; set; }
		public string Revision { get; set; }
		public bool IsSample { get; set; }
		public string[] Conformances { get; } = null;
		
		/// <summary>
		/// The Hash or MAC functions utilized for the KDF
		/// </summary>
		public AuxFunction[] AuxFunctions { get; set; }
		/// <summary>
		/// The pattern used for FixedInputConstruction.
		/// </summary>
		public string FixedInfoPattern { get; set; }
		/// <summary>
		/// The encoding type of the fixedInput
		/// </summary>
		public FixedInfoEncoding[] Encoding { get; set; }
		/// <summary>
		/// Supported lengths of Z 
		/// </summary>
		public MathDomain Z { get; set; }
		/// <summary>
		/// The length of DKM the KDF can produce
		/// </summary>
		public int L { get; set; }
	}
}