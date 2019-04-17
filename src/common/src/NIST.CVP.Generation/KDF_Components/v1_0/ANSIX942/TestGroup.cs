using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; }

        public HashFunction HashAlg { get; set; }
        public int KeyLen { get; set; }
        public AnsiX942Types KdfType { get; set; }
        public int OtherInfoLen { get; set; }
        public int ZzLen { get; set; }
    }
}
