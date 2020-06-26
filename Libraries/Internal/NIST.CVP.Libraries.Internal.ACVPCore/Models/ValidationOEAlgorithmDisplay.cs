using System;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class ValidationOEAlgorithmDisplay
	{
		public long ValidationOEAlgorithmID { get; set; }
		public string AlgorithmDisplayName { get; set; }
		public long OEID { get; set; }
		public string OEDisplay { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
