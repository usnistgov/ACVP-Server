using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "msg")]     // Does this need to be changed to something else?
        public List<BitString> Tuple { get; set; }

        [JsonProperty(PropertyName = "len")]
        public int MessageLength {
            get
            {
                if (Tuple == null) return 0;
                return Tuple.Count;
            }
        }

        [JsonProperty(PropertyName = "customization")]
        public string Customization { get; set; } = "";

        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }

        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength {
            get
            {
                if (Digest == null) return 0;
                return Digest.BitLength;
            }
        }

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
                case "message":
                case "msg":
                    ResultsArray[index].Message = new BitString(value, length, false);
                    return true;
                case "digest":
                case "dig":
                case "md":
                case "output":
                    ResultsArray[index].Digest = new BitString(value, length, false);
                    return true;
            }

            return false;
        }
    }
}
