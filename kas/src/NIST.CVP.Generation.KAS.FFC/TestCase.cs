using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCase : TestCaseBase
    {
        public TestCase() { }

        protected TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source) : base(source)
        {
        }
    }
}