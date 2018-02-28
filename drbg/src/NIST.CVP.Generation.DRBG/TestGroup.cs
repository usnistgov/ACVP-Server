using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DRBG
{
    public class TestGroup : ITestGroup
    {
        /// <summary>
        /// Setting this property also updates the "base" equivalent properties of the class.
        /// </summary>
        [JsonIgnore]
        public DrbgParameters DrbgParameters
        {
            get => _drbgParameters;
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

        public int TestGroupId { get; set; }
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

        private DrbgParameters _drbgParameters = new DrbgParameters();

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            DerFunc = expandoSource.GetTypeFromProperty<bool>("derFunc");
            PredResistance = expandoSource.GetTypeFromProperty<bool>("predResistance");
            ReSeed = expandoSource.GetTypeFromProperty<bool>("reSeed");
            EntropyInputLen = expandoSource.GetTypeFromProperty<int>("entropyInputLen");
            NonceLen = expandoSource.GetTypeFromProperty<int>("nonceLen");
            PersoStringLen = expandoSource.GetTypeFromProperty<int>("persoStringLen");
            AdditionalInputLen = expandoSource.GetTypeFromProperty<int>("additionalInputLen");
            ReturnedBitsLen = expandoSource.GetTypeFromProperty<int>("returnedBitsLen");

            Mode = EnumHelpers.GetEnumFromEnumDescription<DrbgMode>(expandoSource.GetTypeFromProperty<string>("mode"), false);
            
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            name = name.ToLower();

            if (bool.TryParse(value, out var boolVal))
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

            if (!int.TryParse(value, out var intVal))
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
