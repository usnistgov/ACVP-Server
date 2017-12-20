using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }       // Not used
        public bool Deferred { get; set; }          // Not used -- both can be toggled but do not reflect in vectorset
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
        public List<AlgoArrayResponse> ResultsArray { get; set; } = new List<AlgoArrayResponse>();

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

        public TestCase(string message, string digest)
        {
            Message = new BitString(message);
            Digest = new BitString(digest);
        }

        public bool Merge(ITestCase otherTest)
        {
            if(TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase)otherTest;

            if(Message == null && otherTypedTest.Message != null)
            {
                Message = otherTypedTest.Message;
                return true;
            }

            if(Digest == null && otherTypedTest.Digest != null)
            {
                Digest = otherTypedTest.Digest;
                return true;
            }

            if (ResultsArray.Count != 0 && otherTypedTest.ResultsArray.Count != 0)
            {
                return true;
            }

            return false;
        }

        public bool SetString(string name, string value, int length = -1)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "message":
                case "msg":
                    Message = new BitString(value, length);
                    return true;
                case "digest":
                case "dig":
                case "md":
                    Digest = new BitString(value);
                    return true;
            }
            return false;
        }

        public bool SetResultsArrayString(int index, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "message":
                case "msg":
                    ResultsArray[index].Message = new BitString(value);
                    return true;

                case "digest":
                case "dig":
                case "md":
                    ResultsArray[index].Digest = new BitString(value);
                    return true;
            }
            return false;
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            if (((ExpandoObject)source).ContainsProperty("hashFail"))
            {
                FailureTest = source.hashFail;
            }
            if (((ExpandoObject)source).ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (((ExpandoObject)source).ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }
            if (((ExpandoObject) source).ContainsProperty("resultsArray"))
            {
                ResultsArray = ResultsArrayToObject(source.resultsArray);
            }

            Message = BitStringFromObject("msg", (ExpandoObject)source);
            Digest = BitStringFromObject("md", (ExpandoObject)source);
        }

        private List<AlgoArrayResponse> ResultsArrayToObject(dynamic resultsArray)
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();

            foreach (dynamic item in resultsArray)
            {
                var response = new AlgoArrayResponse();
                response.Message = BitStringFromObject("msg", (ExpandoObject) item);
                response.Digest = BitStringFromObject("md", (ExpandoObject) item);

                list.Add(response);
            }

            return list;
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
    }
}

