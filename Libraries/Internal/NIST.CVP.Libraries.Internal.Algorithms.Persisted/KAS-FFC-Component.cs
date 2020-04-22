using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_FFC_Component : PersistedAlgorithmBase
	{
		[AlgorithmProperty("function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_FFC_Component()
		{
			Name = "KAS-FFC";
			Mode = "Component";
			Revision = "1.0";
		}

		public KAS_FFC_Component(External.KAS_FFC_Component external) : this()
		{
			Functions = external.Functions;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "dhEphem", PrependParentPropertyName = true)]
			public Scheme DhEphem { get; set; }

			[AlgorithmProperty(Name = "mqv1", PrependParentPropertyName = true)]
			public Scheme Mqv1 { get; set; }

			[AlgorithmProperty(Name = "dhHybrid1", PrependParentPropertyName = true)]
			public Scheme DhHybrid1 { get; set; }

			[AlgorithmProperty(Name = "mqv2", PrependParentPropertyName = true)]
			public Scheme Mqv2 { get; set; }

			[AlgorithmProperty(Name = "dhHybridOneFlow", PrependParentPropertyName = true)]
			public Scheme DhHybridOneFlow { get; set; }

			[AlgorithmProperty(Name = "dhOneFlow", PrependParentPropertyName = true)]
			public Scheme DhOneFlow { get; set; }

			[AlgorithmProperty(Name = "dhStatic", PrependParentPropertyName = true)]
			public Scheme DhStatic { get; set; }

			public static SchemeCollection Create(External.KAS_FFC_Component.SchemeCollection externalSchemeCollection) => externalSchemeCollection == null ? null : new SchemeCollection
			{
				DhEphem = Scheme.Create(externalSchemeCollection.DhEphem),
				Mqv1 = Scheme.Create(externalSchemeCollection.Mqv1),
				DhHybrid1 = Scheme.Create(externalSchemeCollection.DhHybrid1),
				Mqv2 = Scheme.Create(externalSchemeCollection.Mqv2),
				DhHybridOneFlow = Scheme.Create(externalSchemeCollection.DhHybridOneFlow),
				DhOneFlow = Scheme.Create(externalSchemeCollection.DhOneFlow),
				DhStatic = Scheme.Create(externalSchemeCollection.DhStatic)
			};
		}

		public class Scheme
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> Role { get; set; }

			[AlgorithmProperty(Name = "noKdfNoKc", PrependParentPropertyName = true)]
			public NoKdfNoKc NoKdfNoKc { get; set; }

			public static Scheme Create(External.KAS_FFC_Component.Scheme externalScheme) => externalScheme == null ? null : new Scheme {
				Role = externalScheme.Role,
				NoKdfNoKc = NoKdfNoKc.Create(externalScheme.NoKdfNoKc)
			};
		}

		public class NoKdfNoKc
		{
			[AlgorithmProperty(Name = "parameterSet", PrependParentPropertyName = true)]
			public ParameterSets ParameterSets { get; set; }

			public static NoKdfNoKc Create(External.KAS_FFC_Component.NoKdfNoKc externalKdfNoKc) => externalKdfNoKc == null ? null : new NoKdfNoKc
			{
				ParameterSets = ParameterSets.Create(externalKdfNoKc.ParameterSets)
			};
		}

		public class ParameterSets
		{
			[AlgorithmProperty(Name = "fa", PrependParentPropertyName = true)]
			public ParameterSet FA { get; set; }

			[AlgorithmProperty(Name = "fb", PrependParentPropertyName = true)]
			public ParameterSet FB { get; set; }

			[AlgorithmProperty(Name = "fc", PrependParentPropertyName = true)]
			public ParameterSet FC { get; set; }

			public static ParameterSets Create(External.KAS_FFC_Component.ParameterSets externalParameterSets) => externalParameterSets == null ? null : new ParameterSets
			{
				FB = ParameterSet.Create(externalParameterSets.FB),
				FC = ParameterSet.Create(externalParameterSets.FC),
			};
		}

		public class ParameterSet
		{
			[AlgorithmProperty(Name = "hashAlg", PrependParentPropertyName = true)]
			public List<string> HashAlg { get; set; }

			public static ParameterSet Create(External.KAS_FFC_Component.ParameterSet externalParameterSet) => externalParameterSet == null ? null : new ParameterSet
			{
				HashAlg = externalParameterSet.HashAlg
			};
		}
	}
}
