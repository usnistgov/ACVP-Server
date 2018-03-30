using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            Direction = source.direction;
            KeyLen = source.keyLen;
            PtLen = source.ptLen;
            TweakMode = source.tweakMode;

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        public string Direction { get; set; }
        public int KeyLen { get; set; }
        public int PtLen { get; set; }
        public string TweakMode { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLen = int.Parse(value);
                    return true;

                case "dataunitlen":
                    PtLen = int.Parse(value);
                    return true;

                case "encrypt":
                    Direction = "encrypt";
                    return true;

                case "decrypt":
                    Direction = "decrypt";
                    return true;

                case "direction":
                    Direction = value;
                    return true;
            }
            return false;
        }
    }
}
