using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public EccKeyPair KeyPair { get; set; }

        public TestCase() { }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            ParseKey((ExpandoObject)source);
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            // Nothing to merge from prompt into answers
            return true;
        }

        private void ParseKey(ExpandoObject source)
        {
            BigInteger d, Qx, Qy;

            if (source.ContainsProperty("d"))
            {
                d = source.GetBigIntegerFromProperty("d");
            }

            if (source.ContainsProperty("Qx"))
            {
                Qx = source.GetBigIntegerFromProperty("Qx");
            }

            if (source.ContainsProperty("Qy"))
            {
                Qy = source.GetBigIntegerFromProperty("Qy");
            }

            KeyPair = new EccKeyPair(new EccPoint(Qx, Qy), d);
        }
    }
}
