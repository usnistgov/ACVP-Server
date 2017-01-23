using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCase : ITestCase
    {
        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public BitString Message { get; set; }
        public BitString Digest { get; set; }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase) otherTest;

            if (Message == null && otherTypedTest.Message != null)
            {
                Message = otherTypedTest.Message;
                return true;
            }

            if (Digest == null && otherTypedTest.Digest != null)
            {
                Digest = otherTypedTest.Digest;
                return true;
            }

            return false;
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "message":
                    Message = new BitString(value);
                    return true;
                case "digest":
                    Digest = new BitString(value);
                    return true;
            }
            return false;
        }

        private BitString BitStringFromObject(string sourcePropertyName, ExpandoObject source)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return null;
            }
            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return null;
            }
            var valueAsBitString = sourcePropertyValue as BitString;
            if (valueAsBitString != null)
            {
                return valueAsBitString;
            }
            return new BitString(sourcePropertyValue.ToString());
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int) source.tcId;
            if (((ExpandoObject) source).ContainsProperty("hashFail"))
            {
                FailureTest = source.hashFail;
            }
            if (((ExpandoObject) source).ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (((ExpandoObject) source).ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }

            Message = BitStringFromObject("message", (ExpandoObject) source);
            Digest = BitStringFromObject("digest", (ExpandoObject) source);
        }
    }
}
