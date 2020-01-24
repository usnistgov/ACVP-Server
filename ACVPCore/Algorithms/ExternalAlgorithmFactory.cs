using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ACVPCore.Algorithms.External;

namespace ACVPCore.Algorithms
{
	public static class ExternalAlgorithmFactory
	{
		public static IExternalAlgorithm Deserialize(string algorithmRegistration)
		{
			//Deserialize the name/mode/revision by deserializing just to an AlgorithmBase
			AlgorithmBase algorithmBase = JsonSerializer.Deserialize<AlgorithmBase>(algorithmRegistration);

			//Get the type by a big old switch
			return (algorithmBase.Name, algorithmBase.Mode, algorithmBase.Revision) switch
			{
				("ACVP-AES-CBC", null, "1.0") => JsonSerializer.Deserialize<AES_CBC>(algorithmRegistration),
				("ACVP-AES-CCM", null, "1.0") => JsonSerializer.Deserialize<AES_CCM>(algorithmRegistration),
				("SHA2-224", null, "1.0") => JsonSerializer.Deserialize<SHA2_224>(algorithmRegistration),
				_ => null
			};
		}

	}
}
