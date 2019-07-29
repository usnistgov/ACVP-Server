﻿using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SRTP
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public int AesKeyLength { get; set; }
        public BitString Kdr { get; set; }

        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylength":
                    AesKeyLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
