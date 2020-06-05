using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_FFC_SSC_SP800_56Ar3 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "domainParameterGenerationMethods")]
		public List<string> DomainParameterGenerationMethods { get; set; }

		[AlgorithmProperty(Name = "scheme")] public SchemeCollection Schemes { get; set; }

		public KAS_FFC_SSC_SP800_56Ar3()
		{
			Name = "KAS-FFC-SSC";
			Revision = "Sp800-56Ar3";
		}

		public KAS_FFC_SSC_SP800_56Ar3(External.KAS_FFC_SSC_SP800_56Ar3 external) : this()
		{
			DomainParameterGenerationMethods = external.DomainParameterGenerationMethods;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "dhHybrid1", PrependParentPropertyName = true)]
			public Scheme DhHybrid1 { get; set; }

			[AlgorithmProperty(Name = "mqv2", PrependParentPropertyName = true)]
			public Scheme MQV2 { get; set; }

			[AlgorithmProperty(Name = "dhEphem", PrependParentPropertyName = true)]
			public Scheme DhEphemeral { get; set; }

			[AlgorithmProperty(Name = "dhHybridOneFlow", PrependParentPropertyName = true)]
			public Scheme DhHybridOneFlow { get; set; }

			[AlgorithmProperty(Name = "mqv1", PrependParentPropertyName = true)]
			public Scheme MQV1 { get; set; }

			[AlgorithmProperty(Name = "dhOneFlow", PrependParentPropertyName = true)]
			public Scheme DhOneFlow { get; set; }

			[AlgorithmProperty(Name = "dhStatic", PrependParentPropertyName = true)]
			public Scheme DhStatic { get; set; }

			public static SchemeCollection Create(
				External.KAS_FFC_SSC_SP800_56Ar3.SchemeCollection externalSchemeCollection) =>
				externalSchemeCollection == null
					? null
					: new SchemeCollection
					{
						DhHybrid1 = Scheme.Create(externalSchemeCollection.DhHybrid1),
						MQV2 = Scheme.Create(externalSchemeCollection.MQV2),
						DhEphemeral = Scheme.Create(externalSchemeCollection.DhEphemeral),
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

			public static Scheme Create(External.KAS_FFC_SSC_SP800_56Ar3.Scheme externalScheme) =>
				externalScheme == null
					? null
					: new Scheme
					{
						KasRole = externalScheme.KasRole,
					};
		}
	}
}
