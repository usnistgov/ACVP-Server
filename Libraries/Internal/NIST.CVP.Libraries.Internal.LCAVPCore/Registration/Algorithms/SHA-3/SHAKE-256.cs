using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;
using Range = NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain.Range;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.SHA_3
{
	public class SHAKE_256 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "inBit")]
		public bool BitOrientedInput { get; set; } = false;

		[JsonProperty(PropertyName = "inEmpty")]
		public bool IncludeNull { get; set; } = false;

		[JsonProperty(PropertyName = "outBit")]
		public bool BitOrientedOutput { get; set; } = false;

		[JsonProperty(PropertyName = "outputLen")]
		public List<MathDomain.Range> OutputLength { get; set; } = new List<MathDomain.Range>();

		//[JsonProperty(PropertyName = "digestSize")]
		//public List<int> DigestSize { get; } = new List<int> { 256 };

		public SHAKE_256(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "SHAKE-256")
		{
			if (options.GetValue("SHAKE_NoNull") == "False") IncludeNull = true;
			if (options.GetValue("SHAKE256_Byte") == "False") BitOrientedInput = true;
			if (options.GetValue("SHAKE256_OutputByteOnly") == "False") BitOrientedOutput = true;

			OutputLength.Add(new MathDomain.Range()
			{
				Min = System.Math.Max(ParsingHelper.ParseValueToInteger(options.GetValue("SHAKE256_Output_MinLen")), 16),      //Must be at least 16, so just in case something funny happened force it to 16
				Max = options.GetValue("SHAKE256_OutputMax2TO16") == "True" ? (int)System.Math.Pow(2, 16) : ParsingHelper.ParseValueToInteger(options.GetValue("SHAKE256_Output_MaxLen"))        //Use 2^16 * 8 if they checked the box, otherwise the number provided
			});
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.SHAKE_256
		{
			InBit = BitOrientedInput,
			InEmpty = IncludeNull,
			OutBit = BitOrientedInput,
			OutputLength = ExtensionMethods.ToCoreRangeList(OutputLength)
		};
	}
}
