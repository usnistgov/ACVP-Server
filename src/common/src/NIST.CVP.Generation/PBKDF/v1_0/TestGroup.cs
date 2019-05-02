using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.PBKDF.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        
        public int KeyLength { get; set; }
        public int PasswordLength { get; set; }
        public int SaltLength { get; set; }
        public int IterationCount { get; set; }
    }
}