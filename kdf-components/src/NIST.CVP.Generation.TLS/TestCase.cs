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
            
            MasterSecret = expandoSource.GetBitStringFromProperty("spiResp");
            KeyBlock = expandoSource.GetBitStringFromProperty("sKeySeed");
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

            }

            return false;
        }
    }
}
