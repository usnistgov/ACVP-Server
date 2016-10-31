using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestVectorSet: ITestVectorSet
    {
        public TestVectorSet()
        {
        }

        public string Algorithm { get; set; }
        public bool IsSample { get; set; }
        public List<ITestGroup> TestGroups { get; set; }

        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("ivGen", group.IVGeneration);
                    ((IDictionary<string, object>)updateObject).Add("ivGenMode", group.IVGenerationMode);
                    ((IDictionary<string, object>)updateObject).Add("ivLen", group.IVLength);
                    ((IDictionary<string, object>)updateObject).Add("ptLen", group.PTLength);
                    ((IDictionary<string, object>)updateObject).Add("aadLen", group.AADLength);
                    ((IDictionary<string, object>)updateObject).Add("tagLen", group.TagLength);
                    ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                        ((IDictionary<string, object>)testObject).Add("tag", test.Tag);
                        ((IDictionary<string, object>)testObject).Add("iv", test.IV);
                        ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);
                        tests.Add(testObject);
                    }

                }
            

                return list;
            }
        }

     

        public List<ITestGroup> PromptProjection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<ITestResult> ResultProjection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        
       
    }
}
