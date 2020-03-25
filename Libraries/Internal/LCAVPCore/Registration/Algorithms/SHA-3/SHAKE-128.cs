using System;
using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;
using Range = LCAVPCore.Registration.MathDomain.Range;

namespace LCAVPCore.Registration.Algorithms.SHA_3
{
	public class SHAKE_128 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "inBit")]
		public bool BitOrientedInput { get; set; } = false;

		[JsonProperty(PropertyName = "inEmpty")]
		public bool IncludeNull { get; set; } = false;

		[JsonProperty(PropertyName = "outBit")]
		public bool BitOrientedOutput { get; set; } = false;

		[JsonProperty(PropertyName = "outputLen")]
		public List<Range> OutputLength { get; set; } = new List<Range>();

		//[JsonProperty(PropertyName = "digestSize")]
		//public List<int> DigestSize { get; } = new List<int> { 128 };

		public SHAKE_128(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "SHAKE-128")
		{
			if (options.GetValue("SHAKE_NoNull") == "False") IncludeNull = true;
			if (options.GetValue("SHAKE128_Byte") == "False") BitOrientedInput = true;
			if (options.GetValue("SHAKE128_OutputByteOnly") == "False") BitOrientedOutput = true;

			OutputLength.Add(new Range()
			{
				Min = Math.Max(ParsingHelper.ParseValueToInteger(options.GetValue("SHAKE128_Output_MinLen")), 16),		//Must be at least 16, so just in case something funny happened force it to 16
				Max = options.GetValue("SHAKE128_OutputMax2TO16") == "True" ? (int)Math.Pow(2, 16) : ParsingHelper.ParseValueToInteger(options.GetValue("SHAKE128_Output_MaxLen"))		//Use 2^16 if they checked the box, otherwise the number provided
			});
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.SHAKE_128
		{
			InBit = BitOrientedInput,
			InEmpty = IncludeNull, 
			OutBit = BitOrientedInput,
			OutputLength = ExtensionMethods.ToCoreRangeList(OutputLength)
		};
	}
}
