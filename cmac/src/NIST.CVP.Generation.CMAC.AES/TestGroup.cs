using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.AES
{
    public class TestGroup : TestGroupBase<TestCase>
    {

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            LoadSource(source);
        }

        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength { get; set; }

        public override int GetHashCode()
        {
            return
                $"{Function}|{TestType}|{KeyLength}|{MessageLength}|{MacLength}"
                    .GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherGroup = obj as TestGroup;
            if (otherGroup == null)
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
        }

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                case "klen":
                    KeyLength = intVal;
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal;
                    return true;
            }
            return false;
        }

        protected override void LoadSource(dynamic source)
        {
            TestType = source.testType;
            Function = source.direction;
            KeyLength = source.keyLen;
            MessageLength = source.msgLen;
            MacLength = source.macLen;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
    }
}
