using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_KDF_OneStep_SP800_56Cr1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "l", PrependParentPropertyName = true)]
		public int L { get; set; }
		
		[AlgorithmProperty(Name = "z", PrependParentPropertyName = true)]
		public Domain Z { get; set; }
		
		[AlgorithmProperty(Name = "auxFunctions", PrependParentPropertyName = true)]
		public List<AuxFunction> AuxFunctions { get; set; }

		[AlgorithmProperty(Name = "fixedInfoPattern", PrependParentPropertyName = true)]
		public string FixedInfoPattern { get; set; }

		[AlgorithmProperty(Name = "encoding", PrependParentPropertyName = true)]
		public List<string> Encoding { get; set; }

		public KAS_KDF_OneStep_SP800_56Cr1()
		{
			Name = "KAS-KDF";
			Mode = "OneStep";
			Revision = "Sp800-56Cr1";
		}

		public KAS_KDF_OneStep_SP800_56Cr1(External.KAS_KDF_OneStep_SP800_56Cr1 external) : this()
		{
			L = external.L;
			Z = external.Z;
			FixedInfoPattern = external.FixedInfoPattern;
			Encoding = external.Encoding;
			AuxFunctions = external.AuxFunctions?.Select(x => AuxFunction.Create(x)).ToList();
		}

		public class AuxFunction
		{
			[AlgorithmProperty(Name = "auxFunctionName", PrependParentPropertyName = true)]
			public string AuxFunctionName { get; set; }

			[AlgorithmProperty(Name = "macSaltMethods", PrependParentPropertyName = true)]
			public List<string> MacSaltMethods { get; set; }

			public static AuxFunction Create(External.KAS_KDF_OneStep_SP800_56Cr1.AuxFunction externalAuxFunction) => externalAuxFunction == null ? null : new AuxFunction
			{
				MacSaltMethods = externalAuxFunction.MacSaltMethods,
				AuxFunctionName = externalAuxFunction.AuxFunctionName
			};
		}
	}
}
