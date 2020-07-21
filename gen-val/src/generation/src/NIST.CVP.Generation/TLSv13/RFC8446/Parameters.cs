using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TLSv13.RFC8446
{
	public class Parameters : IParameters
	{
		public int VectorSetId { get; set; }
		public string Algorithm { get; set; }
		public string Mode { get; set; }
		public string Revision { get; set; }
		public bool IsSample { get; set; }
		public string[] Conformances { get; set; }
		
		public HashFunctions[] HmacAlg { get; set; }
	}
}