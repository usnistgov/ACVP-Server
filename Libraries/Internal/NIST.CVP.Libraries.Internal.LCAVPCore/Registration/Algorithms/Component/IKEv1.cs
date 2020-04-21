using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.Component
{
	public class IKEv1 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public IKEv1(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "kdf-components", "ikev1")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KDF_800_135_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("HMAC", options.GetValue("KDF_800_135_Prerequisite_HMAC")));

			//Grab the Initiator and Responder lengths since they are shared. All are a little strange. It is normally a range, but if they only support 1 value they're supposed to put the same value in both parameters
			int minValue, maxValue;
			Domain initiatorNonceLength = new Domain();
			Domain responderNonceLength = new Domain();

			//Initiator
			minValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv1_Ni(min)"));
			maxValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv1_Ni(max)"));

			if (minValue == maxValue)
			{
				initiatorNonceLength.Add(minValue);
			}
			else
			{
				initiatorNonceLength.Add(new Range { Min = minValue, Max = maxValue });
			}

			//Responder
			minValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv1_Nr(min)"));
			maxValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv1_Nr(max)"));

			if (minValue == maxValue)
			{
				responderNonceLength.Add(minValue);
			}
			else
			{
				responderNonceLength.Add(new Range { Min = minValue, Max = maxValue });
			}


			//This is rather annoying and wasteful how this works, but because of the way the new registration is broken out we have to create up to 9 different capabilities with largely redundance information. It will take redundant code too.

			//DSA Capabilities
			if (options.GetValue("KDF_800_135_IKEv1_DigitalSignatureAuthentication") == "True")
			{
				//There may be 1 to 3 DHKeys. Fortunately the keys are all the same except the number, ranging from 0 to 2
				for (int i = 0; i <= 2; i++)
				{
					//Check if the ith DHKey was used
					if (options.GetValue($"KDF_800_135_IKEv1_use_shared_secret_length{i}") == "True")
					{
						Capability dsaCapability = new Capability
						{
							AuthenticationMethod = "dsa",
							InitiatorNonceLength = initiatorNonceLength,
							ResponderNonceLength = responderNonceLength
						};

						//Get the selected shard secret length
						dsaCapability.DHSharedSecretLength.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"KDF_800_135_IKEv1_shared_secret_length{i}")));

						//Check the 5 hash algos and add where appropriate - can't easily loop because difference in strings that get added
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_1") == "True") dsaCapability.HashAlgorithms.Add($"SHA-1");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_224") == "True") dsaCapability.HashAlgorithms.Add($"SHA2-224");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_256") == "True") dsaCapability.HashAlgorithms.Add($"SHA2-256");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_384") == "True") dsaCapability.HashAlgorithms.Add($"SHA2-384");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_512") == "True") dsaCapability.HashAlgorithms.Add($"SHA2-512");

						Capabilities.Add(dsaCapability);
					}
				}
			}

			if (options.GetValue("KDF_800_135_IKEv1_PublicKeyEncryptionAuthentication") == "True")
			{
				//There may be 1 to 3 DHKeys. Fortunately the keys are all the same except the number, ranging from 0 to 2
				for (int i = 0; i <= 2; i++)
				{
					//Check if the ith DHKey was used
					if (options.GetValue($"KDF_800_135_IKEv1_use_shared_secret_length{i}") == "True")
					{
						Capability pkeCapability = new Capability
						{
							AuthenticationMethod = "pke",
							InitiatorNonceLength = initiatorNonceLength,
							ResponderNonceLength = responderNonceLength
						};

						//Get the selected shard secret length
						pkeCapability.DHSharedSecretLength.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"KDF_800_135_IKEv1_shared_secret_length{i}")));

						//Check the 5 hash algos and add where appropriate - can't easily loop because difference in strings that get added
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_1") == "True") pkeCapability.HashAlgorithms.Add($"SHA-1");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_224") == "True") pkeCapability.HashAlgorithms.Add($"SHA2-224");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_256") == "True") pkeCapability.HashAlgorithms.Add($"SHA2-256");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_384") == "True") pkeCapability.HashAlgorithms.Add($"SHA2-384");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_512") == "True") pkeCapability.HashAlgorithms.Add($"SHA2-512");

						Capabilities.Add(pkeCapability);
					}
				}
			}

			if (options.GetValue("KDF_800_135_IKEv1_PreSharedKeyAuthentication") == "True")
			{
				//There may be 1 to 3 DHKeys. Fortunately the keys are all the same except the number, ranging from 0 to 2
				for (int i = 0; i <= 2; i++)
				{
					//Check if the ith DHKey was used
					if (options.GetValue($"KDF_800_135_IKEv1_use_shared_secret_length{i}") == "True")
					{
						//Get the Preshared Key Length object, since it will be the same in all PSK capabilities
						Domain presharedKeyLength = new Domain();

						minValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv1_preshared_key(min)"));
						maxValue = ParsingHelper.ParseValueToInteger(options.GetValue("KDF_800_135_IKEv1_preshared_key(max)"));

						if (minValue == maxValue)
						{
							presharedKeyLength.Add(minValue);
						}
						else
						{
							presharedKeyLength.Add(new Range { Min = minValue, Max = maxValue });
						}


						Capability pskCapability = new Capability
						{
							AuthenticationMethod = "psk",
							InitiatorNonceLength = initiatorNonceLength,
							ResponderNonceLength = responderNonceLength,
							PresharedKeyLength = presharedKeyLength
						};

						//Get the selected shard secret length
						pskCapability.DHSharedSecretLength.Add(ParsingHelper.ParseValueToInteger(options.GetValue($"KDF_800_135_IKEv1_shared_secret_length{i}")));

						//Check the 5 hash algos and add where appropriate - can't easily loop because difference in strings that get added
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_1") == "True") pskCapability.HashAlgorithms.Add($"SHA-1");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_224") == "True") pskCapability.HashAlgorithms.Add($"SHA2-224");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_256") == "True") pskCapability.HashAlgorithms.Add($"SHA2-256");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_384") == "True") pskCapability.HashAlgorithms.Add($"SHA2-384");
						if (options.GetValue($"KDF_800_135_IKEv1_{i}_SHA_512") == "True") pskCapability.HashAlgorithms.Add($"SHA2-512");

						Capabilities.Add(pskCapability);
					}
				}
			}
		}

		public class Capability
		{
			[JsonProperty(PropertyName = "authenticationMethod")]
			public string AuthenticationMethod { get; set; }

			[JsonProperty(PropertyName = "initiatorNonceLength")]
			public Domain InitiatorNonceLength { get; set; }

			[JsonProperty(PropertyName = "responderNonceLength")]
			public Domain ResponderNonceLength { get; set; }

			[JsonProperty(PropertyName = "diffieHellmanSharedSecretLength")]
			public Domain DHSharedSecretLength { get; set; } = new Domain();

			[JsonProperty(PropertyName = "hashAlg")]
			public List<string> HashAlgorithms { get; set; } = new List<string>();

			[JsonProperty(PropertyName = "preSharedKeyLength", NullValueHandling = NullValueHandling.Ignore)]      //This should only be serialized if the psKey method is used, so default this to null and use NullValueHandling to suppress serialization
			public Domain PresharedKeyLength { get; set; } = null;

		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.KDF_IKEv1
		{
			Capabilities = Capabilities.Select(x => new KDF_IKEv1.Capability
			{
				AuthenticationMethod = x.AuthenticationMethod,
				InitiatorNonceLength = x.InitiatorNonceLength.ToCoreDomain(),
				ResponderNonceLength = x.ResponderNonceLength.ToCoreDomain(),
				DiffieHellmanSharedSecretLength = x.DHSharedSecretLength.ToCoreDomain(),
				HashAlgorithms = x.HashAlgorithms,
				PresharedKeyLength = x.PresharedKeyLength.ToCoreDomain()
			}).ToList()
		};
	}
}
