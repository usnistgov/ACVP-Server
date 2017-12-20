using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KDF
{
    public class TestGroup : ITestGroup
    {
        public int KeyOutLength { get; set; }
        public KdfModes KdfMode { get; set; }
        public MacModes MacMode { get; set; }
        public int CounterLength { get; set; }
        public CounterLocations CounterLocation { get; set; }
        public bool ZeroLengthIv { get; set; }

        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;
            KeyOutLength = expandoSource.GetTypeFromProperty<int>("keyOutLength");
            KdfMode = EnumHelpers.GetEnumFromEnumDescription<KdfModes>(expandoSource.GetTypeFromProperty<string>("kdfMode"));
            MacMode = EnumHelpers.GetEnumFromEnumDescription<MacModes>(expandoSource.GetTypeFromProperty<string>("macMode"));
            CounterLocation = EnumHelpers.GetEnumFromEnumDescription<CounterLocations>(expandoSource.GetTypeFromProperty<string>("counterLocation"));
            CounterLength = expandoSource.GetTypeFromProperty<int>("counterLength");
            ZeroLengthIv = expandoSource.GetTypeFromProperty<bool>("zeroLengthIv");

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

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
                $"{KeyOutLength}|{EnumHelpers.GetEnumDescriptionFromEnum(KdfMode)}|{EnumHelpers.GetEnumDescriptionFromEnum(MacMode)}|{CounterLength}|{EnumHelpers.GetEnumDescriptionFromEnum(CounterLocation)}|{ZeroLengthIv}"
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

            switch (name.ToLower())
            {
                case "keyoutlength":
                case "l":
                    KeyOutLength = int.Parse(value);
                    return true;

                case "prf":
                    value = value.Replace("_", "-");
                    MacMode = EnumHelpers.GetEnumFromEnumDescription<MacModes>(value);
                    return true;

                case "ctrlocation":
                    if (value.ToLower().Equals("before_iter"))
                    {
                        CounterLocation = CounterLocations.BeforeIterator;
                        return true;
                    }
                    else if (value.ToLower().Equals("after_iter"))
                    {
                        CounterLocation = CounterLocations.BeforeFixedData;
                        return true;
                    }
                    else if (value.ToLower().Equals("after_fixed"))
                    {
                        CounterLocation = CounterLocations.AfterFixedData;
                        return true;
                    }
                    else if (value.ToLower().Equals("before_fixed"))
                    {
                        CounterLocation = CounterLocations.BeforeFixedData;
                        return true;
                    }
                    else if (value.ToLower().Equals("middle_fixed"))
                    {
                        CounterLocation = CounterLocations.MiddleFixedData;
                        return true;
                    }
                    return false;

                case "rlen":
                    value = value.Split("_")[0];
                    CounterLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
