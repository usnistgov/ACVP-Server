using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_ECC : PersistedAlgorithmBase
	{
		[AlgorithmProperty("function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_ECC()
		{
			Name = "KAS-ECC";
			Revision = "1.0";
		}

		public KAS_ECC(External.KAS_ECC external) : this()
		{
			Functions = external.Functions;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "ephemeralUnified", PrependParentPropertyName = true)]
			public SchemeEphemeralUnified EphemeralUnified { get; set; }

			[AlgorithmProperty(Name = "fullUnified", PrependParentPropertyName = true)]
			public Scheme FullUnified { get; set; }

			[AlgorithmProperty(Name = "fullMqv", PrependParentPropertyName = true)]
			public Scheme FullMQV { get; set; }

			[AlgorithmProperty(Name = "onePassUnified", PrependParentPropertyName = true)]
			public Scheme OnePassUnified { get; set; }

			[AlgorithmProperty(Name = "onePassMqv", PrependParentPropertyName = true)]
			public Scheme OnePassMQV { get; set; }

			[AlgorithmProperty(Name = "onePassDh", PrependParentPropertyName = true)]
			public Scheme OnePassDH { get; set; }

			[AlgorithmProperty(Name = "staticUnified", PrependParentPropertyName = true)]
			public SchemeStaticUnified StaticUnified { get; set; }

			public static SchemeCollection Create(External.KAS_ECC.SchemeCollection externalSchemeCollection) => externalSchemeCollection == null ? null : new SchemeCollection
			{
				EphemeralUnified = SchemeEphemeralUnified.Create(externalSchemeCollection.EphemeralUnified),
				FullUnified = Scheme.Create(externalSchemeCollection.FullUnified),
				FullMQV = Scheme.Create(externalSchemeCollection.FullMQV),
				OnePassUnified = Scheme.Create(externalSchemeCollection.OnePassUnified),
				OnePassMQV = Scheme.Create(externalSchemeCollection.OnePassMQV),
				OnePassDH = Scheme.Create(externalSchemeCollection.OnePassDH),
				StaticUnified = SchemeStaticUnified.Create(externalSchemeCollection.StaticUnified)
			};
		}

		public class Scheme
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> Role { get; set; }

			[AlgorithmProperty(Name = "kdfNoKc", PrependParentPropertyName = true)]
			public KdfNoKc KdfNoKc { get; set; }

			[AlgorithmProperty(Name = "kdfKc", PrependParentPropertyName = true)]
			public KdfKc KdfKc { get; set; }

			public static Scheme Create(External.KAS_ECC.Scheme externalScheme) => externalScheme == null ? null : new Scheme
			{
				Role = externalScheme.Role,
				KdfNoKc = KdfNoKc.Create(externalScheme.KdfNoKc),
				KdfKc = KdfKc.Create(externalScheme.KdfKc)
			};
		}

		public class SchemeEphemeralUnified
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> Role { get; set; }

			[AlgorithmProperty(Name = "kdfNoKc", PrependParentPropertyName = true)]
			public KdfNoKc KdfNoKc { get; set; }

			public static SchemeEphemeralUnified Create(External.KAS_ECC.SchemeEphemeralUnified externalScheme) => externalScheme == null ? null : new SchemeEphemeralUnified
			{
				Role = externalScheme.Role,
				KdfNoKc = KdfNoKcStaticUnified.Create(externalScheme.KdfNoKc)
			};
		}

		public class SchemeStaticUnified
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> Role { get; set; }

			[AlgorithmProperty(Name = "kdfNoKc", PrependParentPropertyName = true)]
			public KdfNoKcStaticUnified KdfNoKc { get; set; }

			[AlgorithmProperty(Name = "kdfKc", PrependParentPropertyName = true)]
			public KdfKcStaticUnified KdfKc { get; set; }

			public static SchemeStaticUnified Create(External.KAS_ECC.SchemeStaticUnified externalScheme) => externalScheme == null ? null : new SchemeStaticUnified
			{
				Role = externalScheme.Role,
				KdfNoKc = KdfNoKcStaticUnified.Create(externalScheme.KdfNoKc),
				KdfKc = KdfKcStaticUnified.Create(externalScheme.KdfKc)
			};
		}



		public class KdfNoKc
		{
			[AlgorithmProperty(Name = "kdfOption", PrependParentPropertyName = true)]
			public KdfOptions KdfOption { get; set; }

			[AlgorithmProperty(Name = "parameterSet", PrependParentPropertyName = true)]
			public ParameterSets ParameterSets { get; set; }


			public static KdfNoKc Create(External.KAS_ECC.KdfNoKc externalKdfNoKc) => externalKdfNoKc == null ? null : new KdfNoKc
			{
				KdfOption = KdfOptions.Create(externalKdfNoKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfNoKc.ParameterSets)
			};
		}

		public class KdfNoKcStaticUnified : KdfNoKc
		{
			[AlgorithmProperty(Name = "dkmNonceTypes", PrependParentPropertyName = true)]
			public List<string> DkmNonceTypes { get; set; }

			public static KdfNoKcStaticUnified Create(External.KAS_ECC.KdfNoKcStaticUnified externalKdfNoKc) => externalKdfNoKc == null ? null : new KdfNoKcStaticUnified
			{
				KdfOption = KdfOptions.Create(externalKdfNoKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfNoKc.ParameterSets),
				DkmNonceTypes = externalKdfNoKc.DkmNonceTypes
			};
		}


		public class KdfKc
		{
			[AlgorithmProperty(Name = "kcOption", PrependParentPropertyName = true)]
			public KcOptions KcOption { get; set; }

			[AlgorithmProperty(Name = "kdfOption", PrependParentPropertyName = true)]
			public KdfOptions KdfOption { get; set; }

			[AlgorithmProperty(Name = "parameterSet", PrependParentPropertyName = true)]
			public ParameterSets ParameterSets { get; set; }

			public static KdfKc Create(External.KAS_ECC.KdfKc externalKdfKc) => externalKdfKc == null ? null : new KdfKc
			{
				KcOption = KcOptions.Create(externalKdfKc.KcOption),
				KdfOption = KdfOptions.Create(externalKdfKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfKc.ParameterSets)
			};
		}

		public class KdfKcStaticUnified : KdfKc
		{
			[AlgorithmProperty(Name = "dkmNonceTypes", PrependParentPropertyName = true)]
			public List<string> DkmNonceTypes { get; set; }

			public static KdfKcStaticUnified Create(External.KAS_ECC.KdfKcStaticUnified externalKdfKc) => externalKdfKc == null ? null : new KdfKcStaticUnified
			{
				KcOption = KcOptions.Create(externalKdfKc.KcOption),
				KdfOption = KdfOptions.Create(externalKdfKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfKc.ParameterSets),
				DkmNonceTypes = externalKdfKc.DkmNonceTypes
			};
		}

		public class ParameterSets
		{
			[AlgorithmProperty(Name = "ea", PrependParentPropertyName = true)]
			public ParameterSetEA EA { get; set; }

			[AlgorithmProperty(Name = "eb", PrependParentPropertyName = true)]
			public ParameterSetEB EB { get; set; }

			[AlgorithmProperty(Name = "ec", PrependParentPropertyName = true)]
			public ParameterSetEC EC { get; set; }

			[AlgorithmProperty(Name = "ed", PrependParentPropertyName = true)]
			public ParameterSetED ED { get; set; }

			[AlgorithmProperty(Name = "ee", PrependParentPropertyName = true)]
			public ParameterSetEE EE { get; set; }

			public static ParameterSets Create(External.KAS_ECC.ParameterSets externalParameterSets) => externalParameterSets == null ? null : new ParameterSets
			{
				EB = ParameterSetEB.Create(externalParameterSets.EB),
				EC = ParameterSetEC.Create(externalParameterSets.EC),
				ED = ParameterSetED.Create(externalParameterSets.ED),
				EE = ParameterSetEE.Create(externalParameterSets.EE)
			};
		}

		public abstract class ParameterSetBase
		{
			[AlgorithmProperty(Name = "curve", PrependParentPropertyName = true)]
			public string Curve { get; set; }

			[AlgorithmProperty(Name = "hashAlg", PrependParentPropertyName = true)]
			public List<string> HashAlg { get; set; }
		}

		public class ParameterSetEA : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsEA MacOption { get; set; }
		}

		public class ParameterSetEB : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsEB MacOption { get; set; }

			public static ParameterSetEB Create(External.KAS_ECC.ParameterSetEB externalParameterSet) => externalParameterSet == null ? null : new ParameterSetEB
			{
				Curve = externalParameterSet.Curve,
				HashAlg = externalParameterSet.HashAlg,
				MacOption = MacOptionsEB.Create(externalParameterSet.MacOption)
			};
		}

		public class ParameterSetEC : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsEC MacOption { get; set; }

			public static ParameterSetEC Create(External.KAS_ECC.ParameterSetEC externalParameterSet) => externalParameterSet == null ? null : new ParameterSetEC
			{
				Curve = externalParameterSet.Curve,
				HashAlg = externalParameterSet.HashAlg,
				MacOption = MacOptionsEC.Create(externalParameterSet.MacOption)
			};
		}

		public class ParameterSetED : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsED MacOption { get; set; }

			public static ParameterSetED Create(External.KAS_ECC.ParameterSetED externalParameterSet) => externalParameterSet == null ? null : new ParameterSetED
			{
				Curve = externalParameterSet.Curve,
				HashAlg = externalParameterSet.HashAlg,
				MacOption = MacOptionsED.Create(externalParameterSet.MacOption)
			};
		}

		public class ParameterSetEE : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsEE MacOption { get; set; }

			public static ParameterSetEE Create(External.KAS_ECC.ParameterSetEE externalParameterSet) => externalParameterSet == null ? null : new ParameterSetEE
			{
				Curve = externalParameterSet.Curve,
				HashAlg = externalParameterSet.HashAlg,
				MacOption = MacOptionsEE.Create(externalParameterSet.MacOption)
			};
		}

		public class KdfOptions
		{
			[AlgorithmProperty(Name = "concatenation", PrependParentPropertyName = true)]
			public string Concatenation { get; set; }

			[AlgorithmProperty(Name = "asn1", PrependParentPropertyName = true)]
			public string Asn1 { get; set; }

			public static KdfOptions Create(External.KAS_ECC.KdfOptions externalKdfOptions) => externalKdfOptions == null ? null : new KdfOptions
			{
				Concatenation = externalKdfOptions.Concatenation,
				Asn1 = externalKdfOptions.Asn1
			};
		}


		public abstract class MacOptionsBase
		{
			[AlgorithmProperty(Name = "AES-CCM", PrependParentPropertyName = true)]
			public MacOptionWithNonceLen AesCcm { get; set; }

			[AlgorithmProperty(Name = "CMAC", PrependParentPropertyName = true)]
			public MacOption Cmac { get; set; }

			//Hmac is kind of a garbage property, only there for some really, really old stuff. Never use it. Just there to match the database
			[AlgorithmProperty(Name = "HMAC", PrependParentPropertyName = true)]
			public MacOptionEmpty Hmac { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-512", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D512 { get; set; }
		}

		public class MacOptionsEA
		{
			//These are pretty goofy... Just empty containers
			[AlgorithmProperty(Name = "AES-CCM", PrependParentPropertyName = true)]
			public MacOptionEmpty AesCcm { get; set; }

			[AlgorithmProperty(Name = "CMAC", PrependParentPropertyName = true)]
			public MacOptionEmpty Cmac { get; set; }

			[AlgorithmProperty(Name = "HMAC", PrependParentPropertyName = true)]
			public MacOptionEmpty Hmac { get; set; }
		}

		public class MacOptionsEB : MacOptionsBase
		{
			[AlgorithmProperty(Name = "HMAC-SHA2-224", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D224 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-256", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D256 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-384", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D384 { get; set; }

			public static MacOptionsEB Create(External.KAS_ECC.MacOptionsEB externalMacOptions) => externalMacOptions == null ? null : new MacOptionsEB
			{
				AesCcm = MacOptionWithNonceLen.Create(externalMacOptions.AesCcm),
				Cmac = MacOption.Create(externalMacOptions.Cmac),
				HmacSha2_D224 = MacOption.Create(externalMacOptions.HmacSha2_D224),
				HmacSha2_D256 = MacOption.Create(externalMacOptions.HmacSha2_D256),
				HmacSha2_D384 = MacOption.Create(externalMacOptions.HmacSha2_D384),
				HmacSha2_D512 = MacOption.Create(externalMacOptions.HmacSha2_D512)
			};
		}

		public class MacOptionsEC : MacOptionsBase
		{
			[AlgorithmProperty(Name = "HMAC-SHA2-256", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D256 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-384", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D384 { get; set; }

			public static MacOptionsEC Create(External.KAS_ECC.MacOptionsEC externalMacOptions) => externalMacOptions == null ? null : new MacOptionsEC
			{
				AesCcm = MacOptionWithNonceLen.Create(externalMacOptions.AesCcm),
				Cmac = MacOption.Create(externalMacOptions.Cmac),
				HmacSha2_D256 = MacOption.Create(externalMacOptions.HmacSha2_D256),
				HmacSha2_D384 = MacOption.Create(externalMacOptions.HmacSha2_D384),
				HmacSha2_D512 = MacOption.Create(externalMacOptions.HmacSha2_D512)
			};
		}

		public class MacOptionsED : MacOptionsBase
		{
			[AlgorithmProperty(Name = "HMAC-SHA2-384", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D384 { get; set; }

			public static MacOptionsED Create(External.KAS_ECC.MacOptionsED externalMacOptions) => externalMacOptions == null ? null : new MacOptionsED
			{
				AesCcm = MacOptionWithNonceLen.Create(externalMacOptions.AesCcm),
				Cmac = MacOption.Create(externalMacOptions.Cmac),
				HmacSha2_D384 = MacOption.Create(externalMacOptions.HmacSha2_D384),
				HmacSha2_D512 = MacOption.Create(externalMacOptions.HmacSha2_D512)
			};
		}

		public class MacOptionsEE : MacOptionsBase
		{
			public static MacOptionsEE Create(External.KAS_ECC.MacOptionsEE externalMacOptions) => externalMacOptions == null ? null : new MacOptionsEE
			{
				AesCcm = MacOptionWithNonceLen.Create(externalMacOptions.AesCcm),
				Cmac = MacOption.Create(externalMacOptions.Cmac),
				HmacSha2_D512 = MacOption.Create(externalMacOptions.HmacSha2_D512)
			};
		}

		public class MacOptionEmpty
		{
			//This is empty because for some reason the database has these as a Composite, but with no children
		}
		public class MacOption
		{
			[AlgorithmProperty(Name = "keyLen", PrependParentPropertyName = true)]
			public Domain KeyLen { get; set; }

			[AlgorithmProperty(Name = "macLen", PrependParentPropertyName = true)]
			public int MacLen { get; set; }

			public static MacOption Create(External.KAS_ECC.MacOption externalMacOption) => externalMacOption == null ? null : new MacOption
			{
				KeyLen = externalMacOption.KeyLen,
				MacLen = externalMacOption.MacLen
			};
		}

		public class MacOptionWithNonceLen : MacOption
		{
			[AlgorithmProperty(Name = "nonceLen", PrependParentPropertyName = true)]
			public int? NonceLen { get; set; }

			public static MacOptionWithNonceLen Create(External.KAS_ECC.MacOptionWithNonceLen externalMacOption) => externalMacOption == null ? null : new MacOptionWithNonceLen
			{
				KeyLen = externalMacOption.KeyLen,
				MacLen = externalMacOption.MacLen,
				NonceLen = externalMacOption.NonceLen
			};
		}


		public class KcOptions
		{
			[AlgorithmProperty(Name = "kcRole", PrependParentPropertyName = true)]
			public List<string> KcRole { get; set; }

			[AlgorithmProperty(Name = "kcType", PrependParentPropertyName = true)]
			public List<string> KcType { get; set; }

			[AlgorithmProperty(Name = "nonceType", PrependParentPropertyName = true)]
			public List<string> NonceType { get; set; }

			public static KcOptions Create(External.KAS_ECC.KcOptions externalKcOptions) => externalKcOptions == null ? null : new KcOptions
			{
				KcRole = externalKcOptions.KcRole,
				KcType = externalKcOptions.KcType,
				NonceType = externalKcOptions.NonceType
			};
		}
	}


}
