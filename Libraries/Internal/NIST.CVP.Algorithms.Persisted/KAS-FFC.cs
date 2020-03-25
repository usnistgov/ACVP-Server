using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class KAS_FFC : PersistedAlgorithmBase
	{
		[AlgorithmProperty("function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_FFC()
		{
			Name = "KAS-FFC";
			Revision = "1.0";
		}

		public KAS_FFC(External.KAS_FFC external) : this()
		{
			Functions = external.Functions;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "dhEphem", PrependParentPropertyName = true)]
			public SchemeNoKC DhEphem { get; set; }

			[AlgorithmProperty(Name = "mqv1", PrependParentPropertyName = true)]
			public SchemeKC Mqv1 { get; set; }

			[AlgorithmProperty(Name = "dhHybrid1", PrependParentPropertyName = true)]
			public SchemeKC DhHybrid1 { get; set; }

			[AlgorithmProperty(Name = "mqv2", PrependParentPropertyName = true)]
			public SchemeKC Mqv2 { get; set; }

			[AlgorithmProperty(Name = "hybridOneFlow", PrependParentPropertyName = true)]
			public SchemeKC DhHybridOneFlow { get; set; }

			[AlgorithmProperty(Name = "dhOneFlow", PrependParentPropertyName = true)]
			public SchemeKC DhOneFlow { get; set; }

			[AlgorithmProperty(Name = "dhStatic", PrependParentPropertyName = true)]
			public SchemeDhStatic DhStatic { get; set; }

			public static SchemeCollection Create(External.KAS_FFC.SchemeCollection externalSchemeCollection) => externalSchemeCollection == null ? null : new SchemeCollection
			{
				DhEphem = SchemeNoKC.Create(externalSchemeCollection.DhEphem),
				Mqv1 = SchemeKC.Create(externalSchemeCollection.Mqv1),
				DhHybrid1 = SchemeKC.Create(externalSchemeCollection.DhHybrid1),
				Mqv2 = SchemeKC.Create(externalSchemeCollection.Mqv2),
				DhHybridOneFlow = SchemeKC.Create(externalSchemeCollection.DhHybridOneFlow),
				DhOneFlow = SchemeKC.Create(externalSchemeCollection.DhOneFlow),
				DhStatic = SchemeDhStatic.Create(externalSchemeCollection.DhStatic)
			};
		}

		public abstract class SchemeBase
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> Role { get; set; }

			[AlgorithmProperty(Name = "kdfNoKc", PrependParentPropertyName = true)]
			public KdfNoKc KdfNoKc { get; set; }
		}

		public class SchemeKC : SchemeBase
		{
			[AlgorithmProperty(Name = "kdfKc", PrependParentPropertyName = true)]
			public KdfKc KdfKc { get; set; }

			public static SchemeKC Create(External.KAS_FFC.SchemeKC externalScheme) => externalScheme == null ? null : new SchemeKC
			{
				Role = externalScheme.Role,
				KdfNoKc = KdfNoKc.Create(externalScheme.KdfNoKc),
				KdfKc = KdfKc.Create(externalScheme.KdfKc)
			};
		}

		public class SchemeNoKC : SchemeBase
		{
			public static SchemeNoKC Create(External.KAS_FFC.SchemeNoKC externalScheme) => externalScheme == null ? null : new SchemeNoKC
			{
				Role = externalScheme.Role,
				KdfNoKc = KdfNoKc.Create(externalScheme.KdfNoKc),
			};
		}

		public class SchemeDhStatic
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> Role { get; set; }

			[AlgorithmProperty(Name = "kdfNoKc", PrependParentPropertyName = true)]
			public KdfNoKcDhStatic KdfNoKc { get; set; }

			[AlgorithmProperty(Name = "kdfKc", PrependParentPropertyName = true)]
			public KdfKcDhStatic KdfKc { get; set; }

			public static SchemeDhStatic Create(External.KAS_FFC.SchemeDhStatic externalScheme) => externalScheme == null ? null : new SchemeDhStatic
			{
				Role = externalScheme.Role,
				KdfNoKc = KdfNoKcDhStatic.Create(externalScheme.KdfNoKc),
				KdfKc = KdfKcDhStatic.Create(externalScheme.KdfKc)
			};
		}

		public abstract class KdfBase
		{
			[AlgorithmProperty(Name = "kdfOption", PrependParentPropertyName = true)]
			public KdfOptions KdfOption { get; set; }

			[AlgorithmProperty(Name = "parameterSet", PrependParentPropertyName = true)]
			public ParameterSets ParameterSets { get; set; }
		}

		public class KdfNoKc : KdfBase
		{
			public static KdfNoKc Create(External.KAS_FFC.KdfNoKc externalKdfNoKc) => externalKdfNoKc == null ? null : new KdfNoKc
			{
				KdfOption = KdfOptions.Create(externalKdfNoKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfNoKc.ParameterSets)
			};
		}

		public class KdfNoKcDhStatic : KdfBase
		{
			[AlgorithmProperty(Name = "dkmNonceTypes", PrependParentPropertyName = true)]
			public List<string> DkmNonceTypes { get; set; }

			public static KdfNoKcDhStatic Create(External.KAS_FFC.KdfNoKcDhStatic externalKdfNoKc) => externalKdfNoKc == null ? null : new KdfNoKcDhStatic
			{
				KdfOption = KdfOptions.Create(externalKdfNoKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfNoKc.ParameterSets),
				DkmNonceTypes = externalKdfNoKc.DkmNonceTypes
			};
		}


		public class KdfKc : KdfBase
		{
			[AlgorithmProperty(Name = "kcOption", PrependParentPropertyName = true)]
			public KcOptions KcOption { get; set; }

			public static KdfKc Create(External.KAS_FFC.KdfKc externalKdfKc) => externalKdfKc == null ? null : new KdfKc {
				KcOption = KcOptions.Create(externalKdfKc.KcOption),
				KdfOption = KdfOptions.Create(externalKdfKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfKc.ParameterSets)
			};
		}

		public class KdfKcDhStatic : KdfKc
		{
			[AlgorithmProperty(Name = "dkmNonceTypes", PrependParentPropertyName = true)]
			public List<string> DkmNonceTypes { get; set; }

			public static KdfKcDhStatic Create(External.KAS_FFC.KdfKcDhStatic externalKdfKc) => externalKdfKc == null ? null : new KdfKcDhStatic
			{
				KcOption = KcOptions.Create(externalKdfKc.KcOption),
				KdfOption = KdfOptions.Create(externalKdfKc.KdfOption),
				ParameterSets = ParameterSets.Create(externalKdfKc.ParameterSets),
				DkmNonceTypes = externalKdfKc.DkmNonceTypes
			};
		}

		public class ParameterSets
		{
			[AlgorithmProperty(Name = "fa", PrependParentPropertyName = true)]
			public ParameterSetFA FA { get; set; }

			[AlgorithmProperty(Name = "fb", PrependParentPropertyName = true)]
			public ParameterSetFB FB { get; set; }

			[AlgorithmProperty(Name = "fc", PrependParentPropertyName = true)]
			public ParameterSetFC FC { get; set; }

			public static ParameterSets Create(External.KAS_FFC.ParameterSets externalParameterSets) => externalParameterSets == null ? null : new ParameterSets
			{
				FB = ParameterSetFB.Create(externalParameterSets.FB),
				FC = ParameterSetFC.Create(externalParameterSets.FC)
			};
		}

		public abstract class ParameterSetBase
		{
			[AlgorithmProperty(Name = "hashAlg", PrependParentPropertyName = true)]
			public List<string> HashAlg { get; set; }
		}

		public class ParameterSetFA : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsFA MacOption { get; set; }
		}

		public class ParameterSetFB : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsFB MacOption { get; set; }

			public static ParameterSetFB Create(External.KAS_FFC.ParameterSetFB externalParameterSet) => externalParameterSet == null ? null : new ParameterSetFB
			{
				HashAlg = externalParameterSet.HashAlg,
				MacOption = MacOptionsFB.Create(externalParameterSet.MacOption)
			};
		}

		public class ParameterSetFC : ParameterSetBase
		{
			[AlgorithmProperty(Name = "macOption", PrependParentPropertyName = true)]
			public MacOptionsFC MacOption { get; set; }

			public static ParameterSetFC Create(External.KAS_FFC.ParameterSetFC externalParameterSet) => externalParameterSet == null ? null : new ParameterSetFC
			{
				HashAlg = externalParameterSet.HashAlg,
				MacOption = MacOptionsFC.Create(externalParameterSet.MacOption)
			};
		}

		public class KdfOptions
		{
			[AlgorithmProperty(Name = "concatenation", PrependParentPropertyName = true)]
			public string Concatenation { get; set; }

			[AlgorithmProperty(Name = "asn1", PrependParentPropertyName = true)]
			public string Asn1 { get; set; }

			public static KdfOptions Create(External.KAS_FFC.KdfOptions externalKdfOptions) => externalKdfOptions == null ? null : new KdfOptions
			{
				Concatenation = externalKdfOptions.Concatenation,
				Asn1 = externalKdfOptions.Asn1
			};
		}

		public class MacOptionsFA
		{
			//These are pretty goofy... Just empty containers
			[AlgorithmProperty(Name = "AES-CCM", PrependParentPropertyName = true)]
			public MacOptionEmpty AesCcm { get; set; }

			[AlgorithmProperty(Name = "CMAC", PrependParentPropertyName = true)]
			public MacOptionEmpty Cmac { get; set; }

			[AlgorithmProperty(Name = "HMAC", PrependParentPropertyName = true)]
			public MacOptionEmpty Hmac { get; set; }
		}


		public abstract class MacOptionsBase
		{
			[AlgorithmProperty(Name = "AES-CCM", PrependParentPropertyName = true)]
			public MacOptionWithNonceLen AesCcm { get; set; }

			[AlgorithmProperty(Name = "CMAC", PrependParentPropertyName = true)]
			public MacOption Cmac { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-256", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D256 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-384", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D384 { get; set; }

			[AlgorithmProperty(Name = "HMAC-SHA2-512", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D512 { get; set; }

			[AlgorithmProperty(Name = "HMAC", PrependParentPropertyName = true)]
			public MacOptionEmpty Hmac { get; set; }
		}


		public class MacOptionsFB : MacOptionsBase
		{
			[AlgorithmProperty(Name = "HMAC-SHA2-224", PrependParentPropertyName = true)]
			public MacOption HmacSha2_D224 { get; set; }


			public static MacOptionsFB Create(External.KAS_FFC.MacOptionsFB externalMacOptions) => externalMacOptions == null ? null : new MacOptionsFB
			{
				AesCcm = MacOptionWithNonceLen.Create(externalMacOptions.AesCcm),
				Cmac = MacOption.Create(externalMacOptions.Cmac),
				HmacSha2_D256 = MacOption.Create(externalMacOptions.HmacSha2_D256),
				HmacSha2_D384 = MacOption.Create(externalMacOptions.HmacSha2_D384),
				HmacSha2_D512 = MacOption.Create(externalMacOptions.HmacSha2_D512),
				HmacSha2_D224 = MacOption.Create(externalMacOptions.HmacSha2_D224)
			};
		}

		public class MacOptionsFC : MacOptionsBase
		{
			public static MacOptionsFC Create(External.KAS_FFC.MacOptionsFC externalMacOptions) => externalMacOptions == null ? null : new MacOptionsFC
			{
				AesCcm = MacOptionWithNonceLen.Create(externalMacOptions.AesCcm),
				Cmac = MacOption.Create(externalMacOptions.Cmac),
				HmacSha2_D256 = MacOption.Create(externalMacOptions.HmacSha2_D256),
				HmacSha2_D384 = MacOption.Create(externalMacOptions.HmacSha2_D384),
				HmacSha2_D512 = MacOption.Create(externalMacOptions.HmacSha2_D512)
			};
		}

		public class MacOptionEmpty
		{
			//This is empty because for some reason the database has these as a Composite, but with no children
		}

		public class MacOptionWithNonceLen : MacOption
		{
			[AlgorithmProperty(Name = "nonceLen", PrependParentPropertyName = true)]
			public int? NonceLen { get; set; }

			public static MacOptionWithNonceLen Create(External.KAS_FFC.MacOptionWithNonceLen externalMacOption) => externalMacOption == null ? null : new MacOptionWithNonceLen
			{
				KeyLen = externalMacOption.KeyLen,
				MacLen = externalMacOption.MacLen,
				NonceLen = externalMacOption.NonceLen
			};
		}

		public class MacOption
		{
			[AlgorithmProperty(Name = "keyLen", PrependParentPropertyName = true)]
			public Domain KeyLen { get; set; }

			[AlgorithmProperty(Name = "macLen", PrependParentPropertyName = true)]
			public int MacLen { get; set; }

			public static MacOption Create(External.KAS_FFC.MacOption externalMacOption) => externalMacOption == null ? null : new MacOption
			{
				KeyLen = externalMacOption.KeyLen,
				MacLen = externalMacOption.MacLen
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

			public static KcOptions Create(External.KAS_FFC.KcOptions externalKcOptions) => externalKcOptions == null ? null : new KcOptions
			{
				KcRole = externalKcOptions.KcRole,
				KcType = externalKcOptions.KcType,
				NonceType = externalKcOptions.NonceType
			};
		}
	}


}
