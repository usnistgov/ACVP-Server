using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCase : ITestCase
    {
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

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }       // Not used
        public bool Deferred { get; set; }          // Not used -- both can be toggled but do not reflect in vectorset
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
        public List<AlgoArrayResponse> ResultsArray { get; set; } = new List<AlgoArrayResponse>();

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

            var expandoSource = (ExpandoObject) source;
            if (expandoSource.ContainsProperty("hashFail"))
            {
                FailureTest = source.hashFail;
            }
            if (expandoSource.ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (expandoSource.ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }
            if (expandoSource.ContainsProperty("resultsArray"))
            {
                ResultsArray = ResultsArrayToObject(source.resultsArray);
            }

            Message = expandoSource.GetBitStringFromProperty("msg");
            Digest = expandoSource.GetBitStringFromProperty("md");
        }

        private List<AlgoArrayResponse> ResultsArrayToObject(dynamic resultsArray)
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();

            foreach (dynamic item in resultsArray)
            {
                var response = new AlgoArrayResponse();
                var expandoItem = (ExpandoObject) item;
                response.Message = expandoItem.GetBitStringFromProperty("msg");
                response.Digest = expandoItem.GetBitStringFromProperty("md");

                list.Add(response);
            }

            return list;
        }
    }
}

