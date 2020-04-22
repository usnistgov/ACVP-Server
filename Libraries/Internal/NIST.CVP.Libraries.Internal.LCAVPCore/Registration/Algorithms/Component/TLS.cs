using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.Component
{
	public class TLS : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "tlsVersion")]
		public List<string> TLSVersions { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "hashAlg", NullValueHandling = NullValueHandling.Ignore)]      //Only applies to TLS 1.2
		public List<string> HashAlgorithms { get; private set; } = null;


		public TLS(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "kdf-components", "tls")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KDF_800_135_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("HMAC", options.GetValue("KDF_800_135_Prerequisite_HMAC")));

			//TLS 1.0/1.1 just adds to the TLSVersions
			if (options.GetValue("KDF_800_135_TLS10_11") == "True") TLSVersions.Add("v1.0/1.1");

			//TLS 1.2 also means that hash algorithms get populated
			if (options.GetValue("KDF_800_135_TLS12") == "True")
			{
				TLSVersions.Add("v1.2");

				//SHA only applies to TLS 1.2
				HashAlgorithms = new List<string>();
				if (options.GetValue("KDF_800_135_TLS12_SHA_256") == "True") HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("KDF_800_135_TLS12_SHA_384") == "True") HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("KDF_800_135_TLS12_SHA_512") == "True") HashAlgorithms.Add("SHA2-512");
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.KDF_TLS
		{
			TLSVersions = TLSVersions,
			HashAlgorithms = HashAlgorithms
		};
	}
}
