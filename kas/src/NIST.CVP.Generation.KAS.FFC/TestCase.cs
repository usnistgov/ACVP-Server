using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCase : TestCaseBase
    {
        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source) : base(source)
        {
        }
    }
}