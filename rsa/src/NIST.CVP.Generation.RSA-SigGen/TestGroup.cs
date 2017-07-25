using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroup : ITestGroup
    {
        public SigGenModes Mode { get; set; }
        public int Modulo { get; set; }
        public HashFunction HashAlg { get; set; }
        public SaltModes SaltMode { get; set; }
        public BitString Salt { get; set; }
        public int SaltLen { get; set; }
        public KeyPair Key { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
        }

        public bool MergeTests(List<ITestCase> testsToMerge)
        {
            return true;
        }
    }
}
