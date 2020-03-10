using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class IKEv2 : AlgorithmBase, IAlgorithm
	{

		[JsonProperty(PropertyName = "capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public IKEv2(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "kdf-components", "ikev2")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KDF_800_135_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("HMAC", options.GetValue("KDF_800_135_Prerequisite_HMAC")));

			//Grab the Initiator and Responder lengths since they are shared. All are a little strange. It is normally a range, but if they only support 1 value they're supposed to put the same value in both parameters
			int minValue, maxValue;
			Domain initiatorNonceLength = new Domain();
			Domain responderNonceLength = new Domain();
			Domain derivedKeyingMaterialLength = new Domain();

			//Initiator
			minValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv2_Ni(min)"));
			maxValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv2_Ni(max)"));

			if (minValue == maxValue)
			{
				initiatorNonceLength.Add(minValue);
			}
			else
			{
				initiatorNonceLength.Add(new Range { Min = minValue, Max = maxValue });
			}

			//Responder
			minValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv2_Nr(min)"));
			maxValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv2_Nr(max)"));

			if (minValue == maxValue)
			{
				responderNonceLength.Add(minValue);
			}
			else
			{
				responderNonceLength.Add(new Range { Min = minValue, Max = maxValue });
			}

			//DKMLength
			minValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv2_DKM(min)"));
			maxValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv2_DKM(max)"));

			if (minValue == maxValue)
			{
				derivedKeyingMaterialLength.Add(minValue);
			}
			else
			{
				derivedKeyingMaterialLength.Add(new Range { Min = minValue, Max = maxValue });
			}


			//There may be 1 to 3 DHKeys. Fortunately the keys are all the same except the number, ranging from 0 to 2
			for (int i = 0; i <= 2; i++)
			{
				//Check if the ith DHKey was used
				if (options.GetValue($"KDF_800_135_IKEv2_use_shared_secret_length{i}") == "True")
				{
					Capability capability = new Capability
					{
						InitiatorNonceLength = initiatorNonceLength,
						ResponderNonceLength = responderNonceLength,
						DerivedKeyingMaterialLength = derivedKeyingMaterialLength
					};

					capability.DHSharedSecretLength.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"KDF_800_135_IKEv2_shared_secret_length{i}")));

					//Check the 5 hash algos and add where appropriate - can't easily loop because difference in strings that get added
					if (options.GetValue($"KDF_800_135_IKEv2_{i}_SHA_1") == "True") capability.HashAlgorithms.Add($"SHA-1");
					if (options.GetValue($"KDF_800_135_IKEv2_{i}_SHA_224") == "True") capability.HashAlgorithms.Add($"SHA2-224");
					if (options.GetValue($"KDF_800_135_IKEv2_{i}_SHA_256") == "True") capability.HashAlgorithms.Add($"SHA2-256");
					if (options.GetValue($"KDF_800_135_IKEv2_{i}_SHA_384") == "True") capability.HashAlgorithms.Add($"SHA2-384");
					if (options.GetValue($"KDF_800_135_IKEv2_{i}_SHA_512") == "True") capability.HashAlgorithms.Add($"SHA2-512");

					Capabilities.Add(capability);
				}
			}
		}

		public class Capability
		{
			[JsonProperty(PropertyName = "initiatorNonceLength")]
			public Domain InitiatorNonceLength { get; set; }

			[JsonProperty(PropertyName = "responderNonceLength")]
			public Domain ResponderNonceLength { get; set; }

			[JsonProperty(PropertyName = "diffieHellmanSharedSecretLength")]
			public Domain DHSharedSecretLength { get; set; } = new Domain();

			[JsonProperty(PropertyName = "hashAlg")]
			public List<string> HashAlgorithms { get; set; } = new List<string>();

			[JsonProperty(PropertyName = "derivedKeyingMaterialLength", NullValueHandling = NullValueHandling.Ignore)]      //This should only be serialized if the psKey method is used, so default this to null and use NullValueHandling to suppress serialization
			public Domain DerivedKeyingMaterialLength { get; set; } = null;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KDF_IKEv2
		{
			Capabilities = Capabilities.Select(x => new KDF_IKEv2.Capability
			{
				InitiatorNonceLength = x.InitiatorNonceLength.ToCoreDomain(),
				ResponderNonceLength = x.ResponderNonceLength.ToCoreDomain(),
				DiffieHellmanSharedSecretLength = x.DHSharedSecretLength.ToCoreDomain(),
				HashAlgorithms = x.HashAlgorithms,
				DerivedKeyingMaterialLength = x.DerivedKeyingMaterialLength.ToCoreDomain()
			}).ToList()
		};
	}
}
