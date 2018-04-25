using System;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.AES
{
    public class TestCase : TestCaseBase<TestGroup, TestCase>
    {
        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "key":
                    Key = new BitString(value);
                    return true;
                case "msg":
                    Message = new BitString(value);
                    return true;
                case "mac":
                    Mac = new BitString(value);
                    return true;

            }
            return false;
        }
    }
}
