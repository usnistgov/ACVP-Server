using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KMAC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        public string TestType { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }

        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }

        [JsonProperty(PropertyName = "inBit")]
        public bool BitOrientedInput { get; set; } = false;

        [JsonProperty(PropertyName = "outBit")]
        public bool BitOrientedOutput { get; set; } = false;

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonProperty(PropertyName = "xof")]
        public bool XOF { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                case "klen":
                    KeyLength = intVal * 8;
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal * 8;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal * 8;
                    return true;
            }
            return false;
        }
    }
}
