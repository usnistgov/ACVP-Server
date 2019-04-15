using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.TLS
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }

        public BitString PreMasterSecret { get; set; }
        public BitString ClientHelloRandom { get; set; }
        public BitString ServerHelloRandom { get; set; }
        public BitString ClientRandom { get; set; }
        public BitString ServerRandom { get; set; }
        public BitString MasterSecret { get; set; }
        public BitString KeyBlock { get; set; }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "serverrandom":
                case "server_random":
                    ServerRandom = new BitString(value);
                    return true;

                case "clientrandom":
                case "client_random":
                    ClientRandom = new BitString(value);
                    return true;

                case "clienthello_random":
                    ClientHelloRandom = new BitString(value);
                    return true;

                case "serverhello_random":
                    ServerHelloRandom = new BitString(value);
                    return true;

                case "pre_master_secret":
                    PreMasterSecret = new BitString(value);
                    return true;

                case "master_secret":
                    MasterSecret = new BitString(value);
                    return true;

                case "key_block":
                    KeyBlock = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
