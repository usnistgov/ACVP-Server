using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class KAS_FFC_SP800_56Ar3 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "domainParameterGenerationMethods")]
		public List<string> DomainParameterGenerationMethods { get; set; }

		[AlgorithmProperty(Name = "function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty(Name = "iutId")]
		public string IUTID { get; set; }

		[AlgorithmProperty(Name = "scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_FFC_SP800_56Ar3()
		{
			Name = "KAS-FFC";
			Revision = "Sp800-56Ar3";
		}

		public KAS_FFC_SP800_56Ar3(External.KAS_FFC_SP800_56Ar3 external) : this()
		{
			DomainParameterGenerationMethods = external.DomainParameterGenerationMethods;
			Functions = external.Functions;
			IUTID = external.IUTID;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "dhHybrid1", PrependParentPropertyName = true)]
			public Scheme DhHybrid1 { get; set; }

			[AlgorithmProperty(Name = "mqv2", PrependParentPropertyName = true)]
			public Scheme MQV2 { get; set; }

			[AlgorithmProperty(Name = "dhEphem", PrependParentPropertyName = true)]
			public SchemeNoKc DhEphemeral { get; set; }

			[AlgorithmProperty(Name = "dhHybridOneFlow", PrependParentPropertyName = true)]
			public Scheme DhHybridOneFlow { get; set; }

			[AlgorithmProperty(Name = "mqv1", PrependParentPropertyName = true)]
			public Scheme MQV1 { get; set; }

			[AlgorithmProperty(Name = "dhOneFlow", PrependParentPropertyName = true)]
			public Scheme DhOneFlow { get; set; }

			[AlgorithmProperty(Name = "dhStatic", PrependParentPropertyName = true)]
			public Scheme DhStatic { get; set; }

			public static SchemeCollection Create(External.KAS_FFC_SP800_56Ar3.SchemeCollection externalSchemeCollection) => externalSchemeCollection == null ? null : new SchemeCollection
			{
				DhHybrid1 = Scheme.Create(externalSchemeCollection.DhHybrid1),
				MQV2 = Scheme.Create(externalSchemeCollection.MQV2),
				DhEphemeral = SchemeNoKc.Create(externalSchemeCollection.DhEphemeral),
				DhHybridOneFlow = Scheme.Create(externalSchemeCollection.DhHybridOneFlow),
				MQV1 = Scheme.Create(externalSchemeCollection.MQV1),
				DhOneFlow = Scheme.Create(externalSchemeCollection.DhOneFlow),
				DhStatic = Scheme.Create(externalSchemeCollection.DhStatic)
			};
		}

		public class Scheme
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> KasRole { get; set; }

			[AlgorithmProperty(Name = "kdfMethods", PrependParentPropertyName = true)]
			public KdfMethods KdfMethods { get; set; }

			[AlgorithmProperty(Name = "keyConfirmationMethod", PrependParentPropertyName = true)]
			public KeyConfirmationMethod KeyConfirmationMethod { get; set; }

			[AlgorithmProperty(Name = "l", PrependParentPropertyName = true)]
			public int L { get; set; }

			public static Scheme Create(External.KAS_FFC_SP800_56Ar3.Scheme externalScheme) => externalScheme == null ? null : new Scheme
			{
				KasRole = externalScheme.KasRole,
				KdfMethods = KdfMethods.Create(externalScheme.KdfMethods),
				KeyConfirmationMethod = KeyConfirmationMethod.Create(externalScheme.KeyConfirmationMethod),
				L = externalScheme.L
			};
		}

		public class SchemeNoKc
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> KasRole { get; set; }

			[AlgorithmProperty(Name = "kdfMethods", PrependParentPropertyName = true)]
			public KdfMethods KdfMethods { get; set; }

			[AlgorithmProperty(Name = "l", PrependParentPropertyName = true)]
			public int L { get; set; }

			public static SchemeNoKc Create(External.KAS_FFC_SP800_56Ar3.SchemeNoKc externalSchemeNoKc) => externalSchemeNoKc == null ? null : new SchemeNoKc
			{
				KasRole = externalSchemeNoKc.KasRole,
				KdfMethods = KdfMethods.Create(externalSchemeNoKc.KdfMethods),
				L = externalSchemeNoKc.L
			};
		}


		public class KdfMethods
		{
			[AlgorithmProperty(Name = "oneStepKdf", PrependParentPropertyName = true)]
			public OneStepKdf OneStepKdf { get; set; }

			[AlgorithmProperty(Name = "twoStepKdf", PrependParentPropertyName = true)]
			public TwoStepKdf TwoStepKdf { get; set; }

			public static KdfMethods Create(External.KAS_FFC_SP800_56Ar3.KdfMethods externalKdfMethods) => externalKdfMethods == null ? null : new KdfMethods
			{
				OneStepKdf = OneStepKdf.Create(externalKdfMethods.OneStepKdf),
				TwoStepKdf = TwoStepKdf.Create(externalKdfMethods.TwoStepKdf)
			};
		}


		public class OneStepKdf
		{
			[AlgorithmProperty(Name = "auxFunctions", PrependParentPropertyName = true)]
			public List<AuxFunction> AuxFunctions { get; set; }

			[AlgorithmProperty(Name = "fixedInfoPattern", PrependParentPropertyName = true)]
			public string FixedInfoPattern { get; set; }

			[AlgorithmProperty(Name = "encoding", PrependParentPropertyName = true)]
			public List<string> Encoding { get; set; }

			public static OneStepKdf Create(External.KAS_FFC_SP800_56Ar3.OneStepKdf externalOneStepKdf) => externalOneStepKdf == null ? null : new OneStepKdf
			{
				FixedInfoPattern = externalOneStepKdf.FixedInfoPattern,
				Encoding = externalOneStepKdf.Encoding,
				AuxFunctions = externalOneStepKdf.AuxFunctions?.Select(x => AuxFunction.Create(x)).ToList()
			};
		}

		public class AuxFunction
		{
			[AlgorithmProperty(Name = "auxFunctionName", PrependParentPropertyName = true)]
			public string AuxFunctionName { get; set; }

			[AlgorithmProperty(Name = "macSaltMethods", PrependParentPropertyName = true)]
			public List<string> MacSaltMethods { get; set; }

			public static AuxFunction Create(External.KAS_FFC_SP800_56Ar3.AuxFunction externalAuxFunction) => externalAuxFunction == null ? null : new AuxFunction
			{
				MacSaltMethods = externalAuxFunction.MacSaltMethods,
				AuxFunctionName = externalAuxFunction.AuxFunctionName
			};
		}

		public class TwoStepKdf
		{
			[AlgorithmProperty(Name = "capabilities", PrependParentPropertyName = true)]
			public List<Capability> Capabilities { get; set; } = new List<Capability>();


			public static TwoStepKdf Create(External.KAS_FFC_SP800_56Ar3.TwoStepKdf externalTwoStepKdf) => externalTwoStepKdf == null ? null : new TwoStepKdf
			{
				Capabilities = externalTwoStepKdf.Capabilities?.Select(x => Capability.Create(x)).ToList()
			};
		}

		public class Capability
		{
			[AlgorithmProperty(Name = "macSaltMethods", PrependParentPropertyName = true)]
			public List<string> MacSaltMethods { get; set; }

			[AlgorithmProperty(Name = "fixedInfoPattern", PrependParentPropertyName = true)]
			public string FixedInfoPattern { get; set; }

			[AlgorithmProperty(Name = "encoding", PrependParentPropertyName = true)]
			public List<string> Encoding { get; set; }

			[AlgorithmProperty(Name = "kdfMode", PrependParentPropertyName = true)]
			public string KdfMode { get; set; }

			[AlgorithmProperty(Name = "macMode", PrependParentPropertyName = true)]
			public List<string> MacMode { get; set; }

			[AlgorithmProperty(Name = "fixedDataOrder", PrependParentPropertyName = true)]
			public List<string> FixedDataOrder { get; set; }

			[AlgorithmProperty(Name = "counterLength", PrependParentPropertyName = true)]
			public List<int> CounterLength { get; set; }

			[AlgorithmProperty(Name = "supportsEmptyIv", PrependParentPropertyName = true)]
			public bool SupportsEmptyIV { get; set; }

			[AlgorithmProperty(Name = "requiresEmptyIv", PrependParentPropertyName = true)]
			public bool RequiresEmptyIV { get; set; }

			[AlgorithmProperty(Name = "supportedLengths", PrependParentPropertyName = true)]
			public Domain SupportedLengths { get; set; }

			public static Capability Create(External.KAS_FFC_SP800_56Ar3.Capability externalCapability) => externalCapability == null ? null : new Capability
			{
				MacSaltMethods = externalCapability.MacSaltMethods,
				FixedInfoPattern = externalCapability.FixedInfoPattern,
				Encoding = externalCapability.Encoding,
				KdfMode = externalCapability.KdfMode,
				MacMode = externalCapability.MacMode,
				FixedDataOrder = externalCapability.FixedDataOrder,
				CounterLength = externalCapability.CounterLength,
				SupportsEmptyIV = externalCapability.SupportsEmptyIV,
				RequiresEmptyIV = externalCapability.RequiresEmptyIV,
				SupportedLengths = externalCapability.SupportedLengths
			};
		}

		public class KeyConfirmationMethod
		{
			[AlgorithmProperty(Name = "keyConfirmationDirections", PrependParentPropertyName = true)]
			public List<string> KeyConfirmationDirections { get; set; }

			[AlgorithmProperty(Name = "keyConfirmationRoles", PrependParentPropertyName = true)]
			public List<string> KeyConfirmationRoles { get; set; }

			[AlgorithmProperty(Name = "macMethods", PrependParentPropertyName = true)]
			public MacMethods MacMethods { get; set; }

			public static KeyConfirmationMethod Create(External.KAS_FFC_SP800_56Ar3.KeyConfirmationMethod externalKeyConfirmationMethod) => externalKeyConfirmationMethod == null ? null : new KeyConfirmationMethod
			{
				KeyConfirmationDirections = externalKeyConfirmationMethod.KeyConfirmationDirections,
				KeyConfirmationRoles = externalKeyConfirmationMethod.KeyConfirmationRoles,
				MacMethods = MacMethods.Create(externalKeyConfirmationMethod.MacMethods)
			};
		}

		public class MacMethods
		{
			[AlgorithmProperty(Name = "CMAC", PrependParentPropertyName = true)]
			public MacMethod CMAC { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-224", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA2_224 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-256", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA2_256 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-384", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA2_384 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-512", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA2_512 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-512224", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA2_512224 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-512256", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA2_512256 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA3-224", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA3_224 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA3-256", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA3_256 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA3-384", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA3_384 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA3-512", PrependParentPropertyName = true)]
			public MacMethod HMAC_SHA3_512 { get; set; }

			[AlgorithmProperty(Name = "KMAC-128", PrependParentPropertyName = true)]
			public MacMethod KMAC_128 { get; set; }

			[AlgorithmProperty(Name = "KMAC-256", PrependParentPropertyName = true)]
			public MacMethod KMAC_256 { get; set; }

			public static MacMethods Create(External.KAS_FFC_SP800_56Ar3.MacMethods externalMacMethods) => externalMacMethods == null ? null : new MacMethods
			{
				CMAC = MacMethod.Create(externalMacMethods.CMAC),
				HMAC_SHA2_224 = MacMethod.Create(externalMacMethods.HMAC_SHA2_224),
				HMAC_SHA2_256 = MacMethod.Create(externalMacMethods.HMAC_SHA2_256),
				HMAC_SHA2_384 = MacMethod.Create(externalMacMethods.HMAC_SHA2_384),
				HMAC_SHA2_512 = MacMethod.Create(externalMacMethods.HMAC_SHA2_512),
				HMAC_SHA2_512224 = MacMethod.Create(externalMacMethods.HMAC_SHA2_512224),
				HMAC_SHA2_512256 = MacMethod.Create(externalMacMethods.HMAC_SHA2_512256),
				HMAC_SHA3_224 = MacMethod.Create(externalMacMethods.HMAC_SHA3_224),
				HMAC_SHA3_256 = MacMethod.Create(externalMacMethods.HMAC_SHA3_256),
				HMAC_SHA3_384 = MacMethod.Create(externalMacMethods.HMAC_SHA3_384),
				HMAC_SHA3_512 = MacMethod.Create(externalMacMethods.HMAC_SHA3_512),
				KMAC_128 = MacMethod.Create(externalMacMethods.KMAC_128),
				KMAC_256 = MacMethod.Create(externalMacMethods.KMAC_256),
			};
		}

		public class MacMethod
		{
			[AlgorithmProperty(Name = "keyLen", PrependParentPropertyName = true)]
			public int KeyLength { get; set; }

			[AlgorithmProperty(Name = "macLen", PrependParentPropertyName = true)]
			public int MacLength { get; set; }

			public static MacMethod Create(External.KAS_FFC_SP800_56Ar3.MacMethod externalMacMethod) => externalMacMethod == null ? null : new MacMethod
			{
				KeyLength = externalMacMethod.KeyLength,
				MacLength = externalMacMethod.MacLength
			};
		}
	}
}
