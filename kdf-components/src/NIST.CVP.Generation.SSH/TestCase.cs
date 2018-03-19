using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SSH
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString K { get; set; }
        public BitString H { get; set; }
        public BitString SessionId { get; set; }
        public BitString InitialIvClient { get; set; }
        public BitString InitialIvServer { get; set; }
        public BitString EncryptionKeyClient { get; set; }
        public BitString EncryptionKeyServer { get; set; }
        public BitString IntegrityKeyClient { get; set; }
        public BitString IntegrityKeyServer { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "k":
                    K = new BitString(value);
                    return true;

                case "h":
                    H = new BitString(value);
                    return true;

                case "session_id":
                    SessionId = new BitString(value);
                    return true;

                case "initial iv (client to server)":
                    InitialIvClient = new BitString(value);
                    return true;

                case "initial iv (server to client)":
                    InitialIvServer = new BitString(value);
                    return true;

                case "encryption key (client to server)":
                    EncryptionKeyClient = new BitString(value);
                    return true;

                case "encryption key (server to client)":
                    EncryptionKeyServer = new BitString(value);
                    return true;

                case "integrity key (client to server)":
                    IntegrityKeyClient = new BitString(value);
                    return true;

                case "integrity key (server to client)":
                    IntegrityKeyServer = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
