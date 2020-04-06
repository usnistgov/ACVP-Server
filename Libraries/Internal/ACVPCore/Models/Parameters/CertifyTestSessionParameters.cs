using System.Collections.Generic;

namespace ACVPCore.Models.Parameters
{
	public class CertifyTestSessionParameters
	{
		public long TestSessionID { get; set; }
		public long? ImplementationID { get; set; }
		public long? OEID { get; set; }
		public List<AlgorithmPrerequisites> Prerequisites { get; set; }

		public class AlgorithmPrerequisites
		{
			public string AlgorithmName { get; set; }
			public string AlgorithmMode { get; set; }
			public List<ValidationReference> ValidationReferences { get; set; }
		}

		public class ValidationReference
		{
			public string AlgorithmFamily { get; set; }
			public string ValidationSource { get; set; }
			public long ValidationNumber { get; set; } 
		}
	}
}
