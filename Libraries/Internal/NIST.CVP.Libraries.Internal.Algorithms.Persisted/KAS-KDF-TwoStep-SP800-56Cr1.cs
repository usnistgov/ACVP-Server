using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_KDF_TwoStep_SP800_56Cr1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "l", PrependParentPropertyName = true)]
		public int L { get; set; }
		
		[AlgorithmProperty(Name = "z", PrependParentPropertyName = true)]
		public Domain Z { get; set; }
		
		[AlgorithmProperty(Name = "capabilities", PrependParentPropertyName = true)]
		public List<TwoStepCapability> Capabilities { get; set; }
		
		public KAS_KDF_TwoStep_SP800_56Cr1()
		{
			Name = "KAS-KDF";
			Mode = "TwoStep";
			Revision = "Sp800-56Cr1";
		}

		public KAS_KDF_TwoStep_SP800_56Cr1(External.KAS_KDF_TwoStep_SP800_56Cr1 external) : this()
		{
			L = external.L;
			Z = external.Z;
			Capabilities = external.Capabilities?.Select(x => TwoStepCapability.Create(x)).ToList();
		}

		public class TwoStepCapability
		{
			[AlgorithmProperty(Name = "macSaltMethods", PrependParentPropertyName = true)]
			public List<string> MacSaltMethods { get; set; }
			
			[AlgorithmProperty(Name = "fixedInfoPattern", PrependParentPropertyName = true)]
			public string FixedInfoPattern { get; set; }
			
			[AlgorithmProperty(Name = "fixedInfoEncoding", PrependParentPropertyName = true)]
			public List<string> FixedInfoEncoding { get; set; }
			
			[AlgorithmProperty(Name = "kdfMode", PrependParentPropertyName = true)]
			public string KDFMode { get; set; }

			[AlgorithmProperty(Name = "macMode", PrependParentPropertyName = true)]
			public List<string> MacMode { get; set; }

			[AlgorithmProperty(Name = "supportedLengths", PrependParentPropertyName = true)]
			public Domain SupportedLengths { get; set; }

			[AlgorithmProperty(Name = "fixedDataOrder", PrependParentPropertyName = true)]
			public List<string> FixedDataOrder { get; set; }

			[AlgorithmProperty(Name = "counterLength", PrependParentPropertyName = true)]
			public List<int> CounterLength { get; set; }

			[AlgorithmProperty(Name = "supportsEmptyIv", PrependParentPropertyName = true)]
			public bool SupportsEmptyIV { get; set; }
			
			[AlgorithmProperty(Name = "requiresEmptyIv", PrependParentPropertyName = true)]
			public bool RequiresEmptyIV { get; set; }
			
			public static TwoStepCapability Create(External.KAS_KDF_TwoStep_SP800_56Cr1.TwoStepCapability externalCapability) => externalCapability == null ? null : new TwoStepCapability
			{
				CounterLength = externalCapability.CounterLength,
				MacMode = externalCapability.MacMode,
				SupportedLengths = externalCapability.SupportedLengths,
				FixedDataOrder = externalCapability.FixedDataOrder,
				FixedInfoEncoding = externalCapability.FixedInfoEncoding,
				FixedInfoPattern = externalCapability.FixedInfoPattern,
				MacSaltMethods = externalCapability.MacSaltMethods,
				KDFMode = externalCapability.KDFMode,
				RequiresEmptyIV = externalCapability.RequiresEmptyIV,
				SupportsEmptyIV = externalCapability.SupportsEmptyIV
			};
		}
	}
}
