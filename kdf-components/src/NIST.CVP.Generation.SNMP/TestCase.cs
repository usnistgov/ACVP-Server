using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SNMP
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }

        public string Password { get; set; }
        public BitString SharedKey { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "password":
                    Password = value;
                    return true;

                case "shared_key":
                    SharedKey = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
