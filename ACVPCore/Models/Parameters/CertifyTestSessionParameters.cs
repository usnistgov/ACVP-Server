using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class CertifyTestSessionParameters
	{
		public long TestSessionID { get; set; }
		public long ImplementationID { get; set; }
		public long OEID { get; set; }
		public List<int> Prerequisites { get; set; }

		public class AlgorithmPrerequisite
		{
			public string Algorithm { get; set; }
			public string Mode { get; set; }
			public List<ValidationReference> ValidationReferences { get; set; }
		}

		public class ValidationReference
		{
			public string AlgorithmFamily { get; set; }
			public string ValidationID { get; set; }
		}
	}
}
