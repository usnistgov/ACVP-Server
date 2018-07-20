using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using System.Linq;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "tuple")]
        public List<BitString> Tuple { get; set; }

        [JsonProperty(PropertyName = "len")]
        public int[] MessageLength {
            get
            {

                if (Tuple == null) return new int[] { 0 };
                var lengths = new int[Tuple.Count];
                for (int i = 0; i < Tuple.Count; i++)
                {
                    lengths[i] = Tuple.ElementAt(i).BitLength;
                }
                return lengths;
            }
        }

        [JsonProperty(PropertyName = "tupleLen")]
        public int TupleLength {
            get
            {
                if (Tuple == null) return 0;
                return Tuple.Count;
            }
        }

        [JsonProperty(PropertyName = "customization")]
        public string Customization { get; set; } = "";

        [JsonProperty(PropertyName = "customizationHex")]
        public BitString CustomizationHex { get; set; }

        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }

        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength { get; set; }

        public List<AlgoArrayResponse> ResultsArray { get; set; }

        public bool SetString(string name, string value, int length = -1)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "digest":
                case "dig":
                case "md":
                case "output":
                    Digest = new BitString(value, length, false);
                    return true;
            }

            return false;
        }

        public bool SetResultsArrayString(int index, string name, string value, int length = -1)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "digest":
                case "dig":
                case "md":
                case "output":
                    ResultsArray[index].Digest = new BitString(value, length, false);
                    return true;
            }

            return false;
        }

        public bool SetResultsArrayList(int index, string name, IEnumerable<string> values, int length = -1)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "tuple":
                case "tup":
                    var tuple = new List<BitString>();
                    foreach (var value in values)
                    {
                        tuple.Add(new BitString(value, length, false));
                    }
                    ResultsArray[index].Tuple = tuple;
                    return true;
            }

            return false;
        }
    }
}
