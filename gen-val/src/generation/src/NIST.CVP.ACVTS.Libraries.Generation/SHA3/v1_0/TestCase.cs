using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
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

        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength
        {
            get
            {
                if (Digest == null) return 0;
                return Digest.BitLength;
            }
        }

        public List<AlgoArrayResponse> ResultsArray { get; set; }

        [JsonProperty(PropertyName = "largeMsg")]
        public LargeBitString LargeMessage { get; set; }

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
                    Message = new BitString(value, length, false);
                    return true;
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
