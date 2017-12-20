using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
{
    public class TestGroup : ITestGroup
    {
        private DrbgParameters _drbgParameters = new DrbgParameters();
        /// <summary>
        /// Setting this property also updates the "base" equivalent properties of the class.
        /// </summary>
        public DrbgParameters DrbgParameters
        {
            get
            {
                return _drbgParameters;
            }
            set
            {
                _drbgParameters = value;

                Mode = _drbgParameters.Mode;

                DerFunc = _drbgParameters.DerFuncEnabled;
                PredResistance = _drbgParameters.PredResistanceEnabled;
                ReSeed = _drbgParameters.ReseedImplemented;

                EntropyInputLen = _drbgParameters.EntropyInputLen;
                NonceLen = _drbgParameters.NonceLen;
                PersoStringLen = _drbgParameters.PersoStringLen;
                AdditionalInputLen = _drbgParameters.AdditionalInputLen;

                ReturnedBitsLen = _drbgParameters.ReturnedBitsLen;
            }
        }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "derFunc")]
        public bool DerFunc { get; set; }
        [JsonProperty(PropertyName = "predResistance")]
        public bool ReSeed { get; set; }
        [JsonProperty(PropertyName = "reSeed")]
        public bool PredResistance { get; set; }
        [JsonProperty(PropertyName = "entropyInputLen")]
        public int EntropyInputLen { get; set; }
        [JsonProperty(PropertyName = "nonceLen")]
        public int NonceLen { get; set; }
        [JsonProperty(PropertyName = "persoStringLen")]
        public int PersoStringLen { get; set; }
        [JsonProperty(PropertyName = "additionalInputLen")]
        public int AdditionalInputLen { get; set; }
        [JsonProperty(PropertyName = "returnedBitsLen")]
        public int ReturnedBitsLen { get; set; }
        [JsonProperty(PropertyName = "mode")]
        public DrbgMode Mode { get; set; }

        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestType = source.testType;
            DerFunc = source.derFunc;
            PredResistance = source.predResistance;
            ReSeed = source.reSeed;
            EntropyInputLen = source.entropyInputLen;
            NonceLen = source.nonceLen;
            PersoStringLen = source.persoStringLen;
            AdditionalInputLen = source.additionalInputLen;
            ReturnedBitsLen = source.returnedBitsLen;

            // TODO maybe fix cast?
            Mode = EnumHelpers.GetEnumFromEnumDescription<DrbgMode>((string)source.mode);

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
                $"{TestType}|{DerFunc}|{PredResistance}|{ReSeed}|{EntropyInputLen}|{NonceLen}|{PersoStringLen}|{AdditionalInputLen}|{ReturnedBitsLen}|{Mode}"
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

            name = name.ToLower();

            bool boolVal;
            if (bool.TryParse(value, out boolVal))
            {
                switch (name)
                {
                    case "predictionresistance":
                        PredResistance = boolVal;
                        DrbgParameters.PredResistanceEnabled = boolVal;
                        return true;
                    default:
                        return false;
                }
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name)
            {
                case "entropyinputlen":
                    EntropyInputLen = intVal;
                    DrbgParameters.EntropyInputLen = intVal;
                    return true;
                case "noncelen":
                    NonceLen = intVal;
                    DrbgParameters.NonceLen = intVal;
                    return true;
                case "persostringlen":
                    PersoStringLen = intVal;
                    DrbgParameters.PersoStringLen = intVal;
                    return true;
                case "additionalinputlen":
                    AdditionalInputLen = intVal;
                    DrbgParameters.AdditionalInputLen = intVal;
                    return true;
                case "returnedbitslen":
                    ReturnedBitsLen = intVal;
                    DrbgParameters.ReturnedBitsLen = intVal;
                    return true;
            }
            return false;
        }

    }
}
