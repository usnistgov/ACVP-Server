using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class KTS_IFC : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty(Name = "iutId")]
		public string IUTID { get; set; }

		[AlgorithmProperty(Name = "scheme")]
		public SchemeCollection Schemes { get; set; }

		public KTS_IFC()
		{
			Name = "KTS-IFC";
			Revision = "Sp800-56Br2";
		}

		public KTS_IFC(External.KTS_IFC external) : this()
		{
			Functions = external.Functions;
			IUTID = external.IUTID;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "KTS-OAEP-basic", PrependParentPropertyName = true)]
			public SchemeBase KTSOAEPBasic { get; set; }

			[AlgorithmProperty(Name = "KTS-OAEP-Party_V-confirmation", PrependParentPropertyName = true)]
			public SchemeWithMacMethods KTSOAEPPartyVConfirmation { get; set; }

			public static SchemeCollection Create(External.KTS_IFC.SchemeCollection externalSchemeCollection) => externalSchemeCollection == null ? null : new SchemeCollection
			{
				KTSOAEPBasic = SchemeBase.Create(externalSchemeCollection.KTSOAEPBasic),
				KTSOAEPPartyVConfirmation = SchemeWithMacMethods.Create(externalSchemeCollection.KTSOAEPPartyVConfirmation)
			};
		}

		public class SchemeBase
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> KasRole { get; set; }

			[AlgorithmProperty(Name = "keyGenerationMethods", PrependParentPropertyName = true)]
			public KeyGenerationMethods KeyGenerationMethods { get; set; }

			[AlgorithmProperty(Name = "ktsMethod", PrependParentPropertyName = true)]
			public KtsMethod KtsMethod { get; set; }

			[AlgorithmProperty(Name = "l", PrependParentPropertyName = true)]
			public int L { get; set; }

			public static SchemeBase Create(External.KTS_IFC.SchemeBase externalScheme) => externalScheme == null ? null : new SchemeBase
			{
				KasRole = externalScheme.KasRole,
				KeyGenerationMethods = KeyGenerationMethods.Create(externalScheme.KeyGenerationMethods),
				KtsMethod = KtsMethod.Create(externalScheme.KtsMethod),
				L = externalScheme.L
			};
		}

		public class SchemeWithMacMethods : SchemeBase
		{
			[AlgorithmProperty(Name = "macMethods", PrependParentPropertyName = true)]
			public MacMethods MacMethods { get; set; }

			public static SchemeWithMacMethods Create(External.KTS_IFC.SchemeWithMacMethods externalSchemeWithMacMethods) => externalSchemeWithMacMethods == null ? null : new SchemeWithMacMethods
			{
				KasRole = externalSchemeWithMacMethods.KasRole,
				KeyGenerationMethods = KeyGenerationMethods.Create(externalSchemeWithMacMethods.KeyGenerationMethods),
				KtsMethod = KtsMethod.Create(externalSchemeWithMacMethods.KtsMethod),
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

			public static KeyGenerationMethods Create(External.KTS_IFC.KeyGenerationMethods externalKeyGenerationMethods) => externalKeyGenerationMethods == null ? null : new KeyGenerationMethods
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

			public static KeyGenerationMethodModulo Create(External.KTS_IFC.KeyGenerationMethodModulo externalKeyGenerationMethodModulo) => externalKeyGenerationMethodModulo == null ? null : new KeyGenerationMethodModulo
			{
				Modulo = externalKeyGenerationMethodModulo.Modulo
			};
		}

		public class KeyGenerationMethodModuloFixedPubExp : KeyGenerationMethodModulo
		{
			[AlgorithmProperty(Name = "fixedPubExp", PrependParentPropertyName = true)]
			public string FixedPublicExponent { get; set; }

			public static KeyGenerationMethodModuloFixedPubExp Create(External.KTS_IFC.KeyGenerationMethodModuloFixedPubExp externalKeyGenerationMethodModuloFixedPubExp) => externalKeyGenerationMethodModuloFixedPubExp == null ? null : new KeyGenerationMethodModuloFixedPubExp
			{
				Modulo = externalKeyGenerationMethodModuloFixedPubExp.Modulo,
				FixedPublicExponent = externalKeyGenerationMethodModuloFixedPubExp.FixedPublicExponent
			};
		}


		public class KtsMethod
		{
			[AlgorithmProperty(Name = "hashAlgs", PrependParentPropertyName = true)]
			public List<string> HashAlgorithms { get; set; }

			[AlgorithmProperty(Name = "supportsNullAssociatedData", PrependParentPropertyName = true)]
			public bool SupportsNullAssociatedData { get; set; }

			[AlgorithmProperty(Name = "associatedDataPattern", PrependParentPropertyName = true)]
			public string AssociatedDataPattern { get; set; }

			[AlgorithmProperty(Name = "encoding", PrependParentPropertyName = true)]
			public List<string> Encoding { get; set; }

			public static KtsMethod Create(External.KTS_IFC.KtsMethod externalKtsMethod) => externalKtsMethod == null ? null : new KtsMethod
			{
				HashAlgorithms = externalKtsMethod.HashAlgorithms,
				SupportsNullAssociatedData = externalKtsMethod.SupportsNullAssociatedData,
				AssociatedDataPattern = externalKtsMethod.AssociatedDataPattern,
				Encoding = externalKtsMethod.Encoding
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

			public static MacMethods Create(External.KTS_IFC.MacMethods externalMacMethods) => externalMacMethods == null ? null : new MacMethods
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

			public static MacMethod Create(External.KTS_IFC.MacMethod externalMacMethod) => externalMacMethod == null ? null : new MacMethod
			{
				KeyLength = externalMacMethod.KeyLength,
				MacLength = externalMacMethod.MacLength
			};
		}
	}
}
