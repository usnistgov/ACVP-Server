using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KDF.v1_0;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS_KDF.TwoStep
{
	public class Parameters : IParameters
	{
		public int VectorSetId { get; set; }
		public string Algorithm { get; set; }
		public string Mode { get; set; }
		public string Revision { get; set; }
		public bool IsSample { get; set; }
		public string[] Conformances { get; set; }

		public TwoStepCapabilities[] Capabilities { get; set; }
		public int L { get; set; }
		public MathDomain Z { get; set; }
	}
	
	public class TwoStepCapabilities : Capability
	{
		public MacSaltMethod[] MacSaltMethods { get; set; }
		/// <summary>
		/// The pattern used for FixedInputConstruction.
		/// </summary>
		public string FixedInfoPattern { get; set; }
		/// <summary>
		/// The encoding type of the fixedInput
		/// </summary>
		public FixedInfoEncoding[] FixedInfoEncoding { get; set; }
	}
}