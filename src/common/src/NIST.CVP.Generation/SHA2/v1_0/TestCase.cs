using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore]
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "len")]
        public int MessageLength
        {
            get
            {
                if (Message == null) return 0;
                return Message.BitLength;
            }
        }

        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }
        public List<AlgoArrayResponse> ResultsArray { get; set; }

        public bool SetString(string name, string value, int length = -1)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "message":
                case "msg":
                    Message = new BitString(value, length);
                    return true;
                case "digest":
                case "dig":
                case "md":
                    Digest = new BitString(value);
                    return true;
            }
            return false;
        }

        public bool SetResultsArrayString(int index, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "message":
                case "msg":
                    ResultsArray[index].Message = new BitString(value);
                    return true;

                case "digest":
                case "dig":
                case "md":
                    ResultsArray[index].Digest = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}

