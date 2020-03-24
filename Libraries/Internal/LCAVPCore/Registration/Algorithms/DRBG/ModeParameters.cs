using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.DRBG
{
	public class ModeParameters
	{
		[JsonProperty("mode")]
		public string Mode { get; set; }

		[JsonProperty("entropyInputLen")]
		public Domain EntropyInputLength { get; set; } = new Domain();

		[JsonProperty("nonceLen")]
		public Domain NonceLength { get; set; } = new Domain();

		[JsonProperty("persoStringLen", NullValueHandling = NullValueHandling.Ignore)]
		public Domain PersonalizationStringLength { get; set; }

		[JsonProperty("additionalInputLen", NullValueHandling = NullValueHandling.Ignore)]
		public Domain AdditionalInputLength { get; set; }

		[JsonProperty("returnedBitsLen")]
		public long ReturnedBitsLength { get; set; }

		[JsonProperty("derFuncEnabled", NullValueHandling = NullValueHandling.Ignore)]
		public bool? DerivationFunction { get; private set; }


		public ModeParameters(string name, string lengths, int outputBlocks, int blockSize)
		{
			Mode = name;
			ReturnedBitsLength = outputBlocks * blockSize;

			//Split out the lengths string. First 2 are Entropy, next 2 None, next 2 Personalization range, last 2 Additional Input range
			string[] splitLengths = lengths.Split(" ".ToCharArray());

			EntropyInputLength.Add(int.Parse(splitLengths[0]));

			NonceLength.Add(int.Parse(splitLengths[2]));


			//Personalization and Additional Input require some more logic since they can be duplicate single values or a range

			//Only provide the Personalization String Length if both min/max were parsed to integers properly
			if (int.TryParse(splitLengths[4], out int persMin) &&
				int.TryParse(splitLengths[5], out int persMax))
			{
				PersonalizationStringLength = new Domain();

				if (persMin == persMax)
				{
					PersonalizationStringLength.Add(persMin);
				}
				else if (persMin < persMax)
				{
					PersonalizationStringLength.Add(new Range { Min = persMin, Max = persMax });
				}
				else
				{
					PersonalizationStringLength.Add(new Range { Min = persMax, Max = persMin });	//They were annoying and reversed the values
				}
			}

			//Only provide the Additional Input Length if both min/max were parsed to integers properly
			if (int.TryParse(splitLengths[6], out int addlMin) &&
				int.TryParse(splitLengths[7], out int addlMax))
			{
				AdditionalInputLength = new Domain();

				if (addlMin == addlMax)
				{
					AdditionalInputLength.Add(addlMin);
				}
				else if (addlMin < addlMax)
				{
					AdditionalInputLength.Add(new Range { Min = addlMin, Max = addlMax });
				}
				else
				{
					AdditionalInputLength.Add(new Range { Min = addlMax, Max = addlMin });		//They were annoying and reversed the values
				}
			}
		}

		public ModeParameters(string name, string lengths, int outputBlocks, int blockSize, bool? derivationFuncEnabled) : this(name, lengths, outputBlocks, blockSize)
		{
			DerivationFunction = derivationFuncEnabled;
		}
	}
}
