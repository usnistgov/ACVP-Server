using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SSH
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool Deferred { get; set; }
        public bool FailureTest { get; set; }

        public BitString K { get; set; }
        public BitString H { get; set; }
        public BitString SessionId { get; set; }
        public BitString InitialIvClient { get; set; }
        public BitString InitialIvServer { get; set; }
        public BitString EncryptionKeyClient { get; set; }
        public BitString EncryptionKeyServer { get; set; }
        public BitString IntegrityKeyClient { get; set; }
        public BitString IntegrityKeyServer { get;set; }

        public bool Merge(ITestCase otherTest)
        {
            throw new NotImplementedException();
        }
    }
}
