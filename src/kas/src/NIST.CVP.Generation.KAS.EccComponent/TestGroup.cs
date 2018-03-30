using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {

        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            MapToProperties(source);
        }

        public int TestGroupId { get; set; }
        public string TestType => "AFT";

        public List<ITestCase> Tests { get; set; } = new List<ITestCase>();

        public Curve CurveName { get; set; }

        private void MapToProperties(dynamic source)
        {
            ExpandoObject expandoSource = (ExpandoObject)source;

            TestGroupId = (int) source.tgId;

            CurveName = EnumHelpers.GetEnumFromEnumDescription<Curve>(
                expandoSource.GetTypeFromProperty<string>("curveName")
            );

            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
    }
}