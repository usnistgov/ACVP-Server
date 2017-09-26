using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestGroup : TestGroupBase<TestCase>
    {
        public int KeyingOption { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            LoadSource(source);
        }

        //public bool MergeTests(List<ITestCase> testsToMerge)
        //{
        //    foreach (var test in Tests)
        //    {
        //        var matchingTest = testsToMerge.FirstOrDefault(t => t.TestCaseId == test.TestCaseId);
        //        if (matchingTest == null)
        //        {
        //            return false;
        //        }
        //        if (!test.Merge(matchingTest))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public override int GetHashCode()
        {
            return
                $"{Function}|{TestType}|{KeyingOption}|{MessageLength}|{MacLength}"
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
                    if (intVal == 3) KeyingOption = 1;
                    else if (intVal == 2) KeyingOption = 2;
                    else throw new ArgumentException($"Cannon parse klen {intVal} to KeyingOption");
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

        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength
        {
            get => 192;
            set {} //there must be a better way to do this
        }


        protected override void LoadSource(dynamic source)
        {
            TestType = source.testType;
            Function = source.direction;
            KeyingOption = source.keyingOption;
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
