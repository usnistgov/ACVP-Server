using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
           
            IVGeneration = source.ivGen;
            IVGenerationMode = source.ivGenMode;
            AADLength = source.aadLen;
            PTLength = source.ptLen;
            IVLength = source.ivLen;
            TagLength = source.tagLen;
            KeyLength = source.keyLen;
            Function = source.direction;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }

        }

        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keylen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "ivlen")]
        public int IVLength { get; set; }
        [JsonProperty(PropertyName = "ivgen")]
        public string IVGeneration { get; set; }
        [JsonProperty(PropertyName = "ivgenmode")]
        public string IVGenerationMode { get; set; }
        [JsonProperty(PropertyName = "ptlen")]
        public int PTLength { get; set; }
        [JsonProperty(PropertyName = "aadlen")]
        public int AADLength { get; set; }
        [JsonProperty(PropertyName = "taglen")]
        public int TagLength { get; set; }
        public List<ITestCase> Tests { get; set; }

        public bool MergeTests(List<ITestCase> testsToMerge)
        {
            foreach (var test in Tests)
            {
                var matchingTest = testsToMerge.FirstOrDefault(t => t.TestCaseId == test.TestCaseId);
                if (matchingTest == null)
                {
                    return false;
                }
                if (!test.Merge(matchingTest))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return
                $"{Function}|{KeyLength}|{IVLength}|{IVGeneration}|{IVGenerationMode}|{PTLength}|{AADLength}|{TagLength}"
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

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLength = intVal;
                    return true;
                case "aadlen":
                    AADLength = intVal;
                    return true;
                case "taglen":
                    TagLength = intVal;
                    return true;
                case "ivlen":
                    IVLength = intVal;
                    return true;
                case "ptlen":
                    PTLength = intVal;
                    return true;
            }
            return false;
        }
    }
}
