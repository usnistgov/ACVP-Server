using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KAS_ECC_SSC_SP800_56Ar3 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "domainParameterGenerationMethods")]
		public List<string> DomainParameterGenerationMethods { get; set; }

		[AlgorithmProperty(Name = "hashFunctionZ")]
		public string HashFunctionZ { get; set; }
		
		[AlgorithmProperty(Name = "scheme")] public SchemeCollection Schemes { get; set; }

		public KAS_ECC_SSC_SP800_56Ar3()
		{
			Name = "KAS-ECC-SSC";
			Revision = "Sp800-56Ar3";
		}

		public KAS_ECC_SSC_SP800_56Ar3(External.KAS_ECC_SSC_SP800_56Ar3 external) : this()
		{
			DomainParameterGenerationMethods = external.DomainParameterGenerationMethods;
			Schemes = SchemeCollection.Create(external.Schemes);
		}

		public class SchemeCollection
		{
			[AlgorithmProperty(Name = "fullUnified", PrependParentPropertyName = true)]
			public Scheme FullUnified { get; set; }

			[AlgorithmProperty(Name = "fullMqv", PrependParentPropertyName = true)]
			public Scheme FullMQV { get; set; }

			[AlgorithmProperty(Name = "ephemeralUnified", PrependParentPropertyName = true)]
			public Scheme EphemeralUnified { get; set; }

			[AlgorithmProperty(Name = "onePassUnified", PrependParentPropertyName = true)]
			public Scheme OnePassUnified { get; set; }

			[AlgorithmProperty(Name = "onePassMqv", PrependParentPropertyName = true)]
			public Scheme OnePassMQV { get; set; }

			[AlgorithmProperty(Name = "onePassDh", PrependParentPropertyName = true)]
			public Scheme OnePassDH { get; set; }

			[AlgorithmProperty(Name = "staticUnified", PrependParentPropertyName = true)]
			public Scheme StaticUnified { get; set; }

			public static SchemeCollection Create(
				External.KAS_ECC_SSC_SP800_56Ar3.SchemeCollection externalSchemeCollection) =>
				externalSchemeCollection == null
					? null
					: new SchemeCollection
					{
						FullUnified = Scheme.Create(externalSchemeCollection.FullUnified),
						FullMQV = Scheme.Create(externalSchemeCollection.FullMQV),
						EphemeralUnified = Scheme.Create(externalSchemeCollection.EphemeralUnified),
						OnePassUnified = Scheme.Create(externalSchemeCollection.OnePassUnified),
						OnePassMQV = Scheme.Create(externalSchemeCollection.OnePassMQV),
						OnePassDH = Scheme.Create(externalSchemeCollection.OnePassDH),
						StaticUnified = Scheme.Create(externalSchemeCollection.StaticUnified),
					};
		}

		public class Scheme
		{
			[AlgorithmProperty(Name = "kasRole", PrependParentPropertyName = true)]
			public List<string> KasRole { get; set; }

			public static Scheme Create(External.KAS_ECC_SSC_SP800_56Ar3.Scheme externalScheme) =>
				externalScheme == null
					? null
					: new Scheme
					{
						KasRole = externalScheme.KasRole,
					};
		}
	}
}
