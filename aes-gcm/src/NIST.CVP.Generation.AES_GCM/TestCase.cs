using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public BitString PlainText { get; set; }
        public BitString AAD { get; set; }
        public BitString IV { get; set; }
        public BitString CipherText { get; set; }
        public BitString Tag { get; set; }

    }
}
