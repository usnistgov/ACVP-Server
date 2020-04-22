using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_ECC_Component : PersistedAlgorithmBase
	{
		[AlgorithmProperty("function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_ECC_Component()
		{
			Name = "KAS-ECC";
			Mode = "Component";
			Revision = "1.0";
		}

		public KAS_ECC_Component(External.KAS_ECC_Component external) : this()
		{
			Functions = external.Functions;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "ephemeralUnified", PrependParentPropertyName = true)]
			public Scheme EphemeralUnified { get; set; }

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
			public Scheme StaticUnified { get; set; }

			public static SchemeCollection Create(External.KAS_ECC_Component.SchemeCollection externalSchemeCollection) => externalSchemeCollection == null ? null : new SchemeCollection
			{
				EphemeralUnified = Scheme.Create(externalSchemeCollection.EphemeralUnified),
				FullUnified = Scheme.Create(externalSchemeCollection.FullUnified),
				FullMQV = Scheme.Create(externalSchemeCollection.FullMQV),
				OnePassUnified = Scheme.Create(externalSchemeCollection.OnePassUnified),
				OnePassMQV = Scheme.Create(externalSchemeCollection.OnePassMQV),
				OnePassDH = Scheme.Create(externalSchemeCollection.OnePassDH),
				StaticUnified = Scheme.Create(externalSchemeCollection.StaticUnified)
			};
		}

		public class Scheme
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> Role { get; set; }

			[AlgorithmProperty(Name = "noKdfNoKc", PrependParentPropertyName = true)]
			public NoKdfNoKc NoKdfNoKc { get; set; }

			public static Scheme Create(External.KAS_ECC_Component.Scheme externalScheme) => externalScheme == null ? null : new Scheme {
				Role = externalScheme.Role,
				NoKdfNoKc = NoKdfNoKc.Create(externalScheme.NoKdfNoKc)
			};
		}

		public class NoKdfNoKc
		{
			[AlgorithmProperty(Name = "parameterSet", PrependParentPropertyName = true)]
			public ParameterSets ParameterSets { get; set; }

			public static NoKdfNoKc Create(External.KAS_ECC_Component.NoKdfNoKc externalKdfNoKc) => externalKdfNoKc == null ? null : new NoKdfNoKc
			{
				ParameterSets = ParameterSets.Create(externalKdfNoKc.ParameterSets)
			};
		}

		public class ParameterSets
		{
			[AlgorithmProperty(Name = "ea", PrependParentPropertyName = true)]
			public ParameterSet EA { get; set; }

			[AlgorithmProperty(Name = "eb", PrependParentPropertyName = true)]
			public ParameterSet EB { get; set; }

			[AlgorithmProperty(Name = "ec", PrependParentPropertyName = true)]
			public ParameterSet EC { get; set; }

			[AlgorithmProperty(Name = "ed", PrependParentPropertyName = true)]
			public ParameterSet ED { get; set; }

			[AlgorithmProperty(Name = "ee", PrependParentPropertyName = true)]
			public ParameterSet EE { get; set; }

			public static ParameterSets Create(External.KAS_ECC_Component.ParameterSets externalParameterSets) => externalParameterSets == null ? null : new ParameterSets
			{
				EB = ParameterSet.Create(externalParameterSets.EB),
				EC = ParameterSet.Create(externalParameterSets.EC),
				ED = ParameterSet.Create(externalParameterSets.ED),
				EE = ParameterSet.Create(externalParameterSets.EE)
			};
		}

		public class ParameterSet
		{
			[AlgorithmProperty(Name = "curve", PrependParentPropertyName = true)]
			public string Curve { get; set; }

			[AlgorithmProperty(Name = "hashAlg", PrependParentPropertyName = true)]
			public List<string> HashAlg { get; set; }

			public static ParameterSet Create(External.KAS_ECC_Component.ParameterSet externalParameterSet) => externalParameterSet == null ? null : new ParameterSet
			{
				Curve = externalParameterSet.Curve,
				HashAlg = externalParameterSet.HashAlg
			};
		}
	}
}
