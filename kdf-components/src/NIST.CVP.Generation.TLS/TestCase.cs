using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TLS
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString PreMasterSecret { get; set; }
        public BitString ClientHelloRandom { get; set; }
        public BitString ServerHelloRandom { get; set; }
        public BitString ClientRandom { get; set; }
        public BitString ServerRandom { get; set; }
        public BitString MasterSecret { get; set; }
        public BitString KeyBlock { get; set; }

        public TestCase() { }
        
        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            var expandoSource = (ExpandoObject) source;
            
            PreMasterSecret = expandoSource.GetBitStringFromProperty("preMasterSecret");
            ClientHelloRandom = expandoSource.GetBitStringFromProperty("clientHelloRandom");
            ServerHelloRandom = expandoSource.GetBitStringFromProperty("serverHelloRandom");
            ClientRandom = expandoSource.GetBitStringFromProperty("clientRandom");
            ServerRandom = expandoSource.GetBitStringFromProperty("serverRandom");
            
            MasterSecret = expandoSource.GetBitStringFromProperty("masterSecret");
            KeyBlock = expandoSource.GetBitStringFromProperty("keyBlock");
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            // Nothing to merge, put everything in answer.json
            return true;
        }

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
