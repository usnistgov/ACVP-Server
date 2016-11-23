using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using System.Collections;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestVectorSet: ITestVectorSet
    {
        public TestVectorSet()
        {
        }

        public TestVectorSet(dynamic answers, dynamic prompts)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);
                
                TestGroups.Add(group);
            }

            foreach (var prompt in prompts.testGroups)
            {
                var promptGroup = new TestGroup(prompt);
                var matchingAnswerGroup = TestGroups.FirstOrDefault(g => g.Equals(promptGroup));
                if (matchingAnswerGroup != null)
                {
                    if (!matchingAnswerGroup.MergeTests(promptGroup.Tests))
                    {
                        throw new Exception("Could not reconstitute TestVectorSet from supplied answers and prompts");
                    }
                }

            }


        }

        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        /// <summary>
        /// Expected answers (not sent to client)
        /// </summary>
        public List<dynamic> AnswerProjection
        {
            get
            {
                //@@@this is for encrypt, will need to deal with decrypt
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
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
                        ((IDictionary<string, object>)testObject).Add("key", test.Key);
                        ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);
                        tests.Add(testObject);
                    }
                    list.Add(updateObject);
                    
                }

                return list;
            }
        }

        /// <summary>
        /// What the client receives (should not include expected answers)
        /// </summary>
        [JsonProperty(PropertyName = "testGroups")]
        public List<dynamic> PromptProjection
        {
            //@@@this is for encrypt, will need to deal with decrypt
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
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
                        ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                        ((IDictionary<string, object>)testObject).Add("key", test.Key);
                        ((IDictionary<string, object>)testObject).Add("iv", test.IV);
                        tests.Add(testObject);
                    }
                    list.Add(updateObject);
                }


                return list;
            }
        }

             /// <summary>
        /// Debug projection (internal), as well as potentially sample projection (sent to client)
        /// </summary>
        
        [JsonProperty(PropertyName = "testResults")]
        public dynamic ResultProjection
        {
            get
            {
                //@@@this is for encrypt, will need to deal with decrypt
                var tests = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                        ((IDictionary<string, object>)testObject).Add("tag", test.Tag);
                        ((IDictionary<string, object>)testObject).Add("iv", test.IV);
                        tests.Add(testObject);
                    }
                }
                return tests;
            }

        }

        public dynamic ToDynamic()
        {
            dynamic vectorSetObject = new ExpandoObject();
            ((IDictionary<string, object>)vectorSetObject).Add("answerProjection", AnswerProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("testGroups", PromptProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("resultProjection", ResultProjection);
            return vectorSetObject;
        }
    }
}
