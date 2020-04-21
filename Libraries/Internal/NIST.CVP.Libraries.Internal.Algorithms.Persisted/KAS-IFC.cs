using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_IFC : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty(Name = "iutId")]
		public string IUTID { get; set; }

		[AlgorithmProperty(Name = "scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_IFC()
		{
			Name = "KAS-IFC";
			Revision = "Sp800-56Br2";
		}

		public KAS_IFC(External.KAS_IFC external) : this()
		{
			Functions = external.Functions;
			IUTID = external.IUTID;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "KAS1-basic", PrependParentPropertyName = true)]
			public SchemeBase KAS1Basic { get; set; }

			[AlgorithmProperty(Name = "KAS2-basic", PrependParentPropertyName = true)]
			public SchemeBase KAS2Basic { get; set; }

			[AlgorithmProperty(Name = "KAS1-Party_V-confirmation", PrependParentPropertyName = true)]
			public SchemeWithMacMethods KAS1PartyVConfirmation { get; set; }

			[AlgorithmProperty(Name = "KAS2-bilateral-confirmation", PrependParentPropertyName = true)]
			public SchemeWithMacMethods KAS2BilateralConfirmation { get; set; }

			[AlgorithmProperty(Name = "KAS2-Party_U-confirmation", PrependParentPropertyName = true)]
			public SchemeWithMacMethods KAS2PartyUConfirmation { get; set; }

			[AlgorithmProperty(Name = "KAS2-Party_V-confirmation", PrependParentPropertyName = true)]
			public SchemeWithMacMethods KAS2PartyVConfirmation { get; set; }

			public static SchemeCollection Create(External.KAS_IFC.SchemeCollection externalSchemeCollection) => externalSchemeCollection == null ? null : new SchemeCollection
			{
				KAS1Basic = SchemeBase.Create(externalSchemeCollection.KAS1Basic),
				KAS2Basic = SchemeBase.Create(externalSchemeCollection.KAS2Basic),
				KAS1PartyVConfirmation = SchemeWithMacMethods.Create(externalSchemeCollection.KAS1PartyVConfirmation),
				KAS2BilateralConfirmation = SchemeWithMacMethods.Create(externalSchemeCollection.KAS2BilateralConfirmation),
				KAS2PartyUConfirmation = SchemeWithMacMethods.Create(externalSchemeCollection.KAS2PartyUConfirmation),
				KAS2PartyVConfirmation = SchemeWithMacMethods.Create(externalSchemeCollection.KAS2PartyVConfirmation)
			};
		}

		public class SchemeBase
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> KasRole { get; set; }

			[AlgorithmProperty(Name = "keyGenerationMethods", PrependParentPropertyName = true)]
			public KeyGenerationMethods KeyGenerationMethods { get; set; }

			[AlgorithmProperty(Name = "kdfMethods", PrependParentPropertyName = true)]
			public KdfMethods KdfMethods { get; set; }

			[AlgorithmProperty(Name = "l", PrependParentPropertyName = true)]
			public int L { get; set; }

			public static SchemeBase Create(External.KAS_IFC.SchemeBase externalScheme) => externalScheme == null ? null : new SchemeBase
			{
				KasRole = externalScheme.KasRole,
				KeyGenerationMethods = KeyGenerationMethods.Create(externalScheme.KeyGenerationMethods),
				KdfMethods = KdfMethods.Create(externalScheme.KdfMethods),
				L = externalScheme.L
			};
		}

		public class SchemeWithMacMethods : SchemeBase
		{
			[AlgorithmProperty(Name = "macMethods", PrependParentPropertyName = true)]
			public MacMethods MacMethods { get; set; }

			public static SchemeWithMacMethods Create(External.KAS_IFC.SchemeWithMacMethods externalSchemeWithMacMethods) => externalSchemeWithMacMethods == null ? null : new SchemeWithMacMethods
			{
				KasRole = externalSchemeWithMacMethods.KasRole,
				KeyGenerationMethods = KeyGenerationMethods.Create(externalSchemeWithMacMethods.KeyGenerationMethods),
				KdfMethods = KdfMethods.Create(externalSchemeWithMacMethods.KdfMethods),
				L = externalSchemeWithMacMethods.L,
				MacMethods = MacMethods.Create(externalSchemeWithMacMethods.MacMethods),
			};
		}

		public class KeyGenerationMethods
		{
			[AlgorithmProperty(Name = "rsakpg1-basic", PrependParentPropertyName = true)]
			public KeyGenerationMethodModuloFixedPubExp RSAKpg1Basic { get; set; }

			[AlgorithmProperty(Name = "rsakpg1-prime-factor", PrependParentPropertyName = true)]
			public KeyGenerationMethodModuloFixedPubExp RSAKpg1PrimeFactor { get; set; }

			[AlgorithmProperty(Name = "rsakpg1-crt", PrependParentPropertyName = true)]
			public KeyGenerationMethodModuloFixedPubExp RSAKpg1Crt { get; set; }

			[AlgorithmProperty(Name = "rsakpg2-basic", PrependParentPropertyName = true)]
			public KeyGenerationMethodModulo RSAKpg2Basic { get; set; }

			[AlgorithmProperty(Name = "rsakpg2-prime-factor", PrependParentPropertyName = true)]
			public KeyGenerationMethodModulo RSAKpg2PrimeFactor { get; set; }

			[AlgorithmProperty(Name = "rsakpg2-crt", PrependParentPropertyName = true)]
			public KeyGenerationMethodModulo RSAKpg2Crt { get; set; }

			public static KeyGenerationMethods Create(External.KAS_IFC.KeyGenerationMethods externalKeyGenerationMethods) => externalKeyGenerationMethods == null ? null : new KeyGenerationMethods
			{
				RSAKpg1Basic = KeyGenerationMethodModuloFixedPubExp.Create(externalKeyGenerationMethods.RSAKpg1Basic),
				RSAKpg1PrimeFactor = KeyGenerationMethodModuloFixedPubExp.Create(externalKeyGenerationMethods.RSAKpg1PrimeFactor),
				RSAKpg1Crt = KeyGenerationMethodModuloFixedPubExp.Create(externalKeyGenerationMethods.RSAKpg1Crt),
				RSAKpg2Basic = KeyGenerationMethodModulo.Create(externalKeyGenerationMethods.RSAKpg2Basic),
				RSAKpg2PrimeFactor = KeyGenerationMethodModulo.Create(externalKeyGenerationMethods.RSAKpg2PrimeFactor),
				RSAKpg2Crt = KeyGenerationMethodModulo.Create(externalKeyGenerationMethods.RSAKpg2Crt)
			};
		}

		public class KeyGenerationMethodModulo
		{
			[AlgorithmProperty(Name = "modulo", PrependParentPropertyName = true)]
			public List<int> Modulo { get; set; }

			public static KeyGenerationMethodModulo Create(External.KAS_IFC.KeyGenerationMethodModulo externalKeyGenerationMethodModulo) => externalKeyGenerationMethodModulo == null ? null : new KeyGenerationMethodModulo
			{
				Modulo = externalKeyGenerationMethodModulo.Modulo
			};
		}

		public class KeyGenerationMethodModuloFixedPubExp : KeyGenerationMethodModulo
		{
			[AlgorithmProperty(Name = "fixedPubExp", PrependParentPropertyName = true)]
			public string FixedPublicExponent { get; set; }

			public static KeyGenerationMethodModuloFixedPubExp Create(External.KAS_IFC.KeyGenerationMethodModuloFixedPubExp externalKeyGenerationMethodModuloFixedPubExp) => externalKeyGenerationMethodModuloFixedPubExp == null ? null : new KeyGenerationMethodModuloFixedPubExp
			{
				Modulo = externalKeyGenerationMethodModuloFixedPubExp.Modulo,
				FixedPublicExponent = externalKeyGenerationMethodModuloFixedPubExp.FixedPublicExponent
			};
		}


		public class KdfMethods
		{
			[AlgorithmProperty(Name = "oneStepKdf", PrependParentPropertyName = true)]
			public OneStepKdf OneStepKdf { get; set; }

			[AlgorithmProperty(Name = "twoStepKdf", PrependParentPropertyName = true)]
			public TwoStepKdf TwoStepKdf { get; set; }

			public static KdfMethods Create(External.KAS_IFC.KdfMethods externalKdfMethods) => externalKdfMethods == null ? null : new KdfMethods
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

			public static OneStepKdf Create(External.KAS_IFC.OneStepKdf externalOneStepKdf) => externalOneStepKdf == null ? null : new OneStepKdf
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

			public static AuxFunction Create(External.KAS_IFC.AuxFunction externalAuxFunction) => externalAuxFunction == null ? null : new AuxFunction
			{
				MacSaltMethods = externalAuxFunction.MacSaltMethods,
				AuxFunctionName = externalAuxFunction.AuxFunctionName
			};
		}

		public class TwoStepKdf
		{
			[AlgorithmProperty(Name = "capabilities", PrependParentPropertyName = true)]
			public List<Capability> Capabilities { get; set; } = new List<Capability>();

			public static TwoStepKdf Create(External.KAS_IFC.TwoStepKdf externalTwoStepKdf) => externalTwoStepKdf == null ? null : new TwoStepKdf
			{
				Capabilities = externalTwoStepKdf.Capabilities?.Select(x => Capability.Create(x)).ToList(),
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

			public static Capability Create(External.KAS_IFC.Capability externalCapability) => externalCapability == null ? null : new Capability
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

			public static MacMethods Create(External.KAS_IFC.MacMethods externalMacMethods) => externalMacMethods == null ? null : new MacMethods
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

			public static MacMethod Create(External.KAS_IFC.MacMethod externalMacMethod) => externalMacMethod == null ? null : new MacMethod
			{
				KeyLength = externalMacMethod.KeyLength,
				MacLength = externalMacMethod.MacLength
			};
		}
	}
}
