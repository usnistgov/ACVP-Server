using Newtonsoft.Json;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NIST.CVP.Generation.TDES_CBCI
{
    public class TestVectorSet : ITestVectorSet
    {
        private readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions =
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty,
                PrintOptionBitStringEmpty.PrintAsEmptyString);


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
                var matchingAnswerGroup = TestGroups.Single(g => g.Equals(promptGroup));
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
        public string Mode { get; set; } = string.Empty;
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
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("keyingOption", group.KeyingOption);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.Equals("MCT", StringComparison.OrdinalIgnoreCase))
                        {
                            var resultsArray = new List<dynamic>();

                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();

                                var keys = new TDESKeys(result.Keys);
                                for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    ((IDictionary<string, object>)resultObject).Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                                }

                                ((IDictionary<string, object>)resultObject).Add("iv1", result.IV1);
                                ((IDictionary<string, object>)resultObject).Add("iv2", result.IV2);
                                ((IDictionary<string, object>)resultObject).Add("iv3", result.IV3);
                                ((IDictionary<string, object>)resultObject).Add("pt", result.PlainText);
                                ((IDictionary<string, object>)resultObject).Add("ct", result.CipherText);

                                resultsArray.Add(resultObject);
                            }
                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            AddToObjectIfNotNull(testObject, "key1", test.Key1);
                            AddToObjectIfNotNull(testObject, "key2", test.Key2);
                            AddToObjectIfNotNull(testObject, "key3", test.Key3);

                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                AddToObjectIfNotNull(testObject, "ct", test.CipherText);
                            }

                            else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                AddToObjectIfNotNull(testObject, "pt", test.PlainText);
                            }




                            ((IDictionary<string, object>)testObject).Add("iv1", test.IV1);
                            ((IDictionary<string, object>)testObject).Add("iv2", test.IV2);
                            ((IDictionary<string, object>)testObject).Add("iv3", test.IV3);
                        }

                        ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);
                        tests.Add(testObject);
                    }
                    list.Add(updateObject);

                }

                return list;
            }
        }

        private void AddToObjectIfNotNull(dynamic obj, string propertyName, object propertyValue)
        {
            if (propertyValue != null)
            {
                ((IDictionary<string, object>)obj).Add(propertyName, propertyValue);
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

        /// <summary>
        /// What the client receives (should not include expected answers)
        /// </summary>
        [JsonProperty(PropertyName = "testGroups")]
        public List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("keyingOption", group.KeyingOption);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        AddToObjectIfNotNull(testObject, "key1", test.Key1);
                        AddToObjectIfNotNull(testObject, "key2", test.Key2);
                        AddToObjectIfNotNull(testObject, "key3", test.Key3);

                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            AddToObjectIfNotNull(testObject, "pt", test.PlainText);
                        }
                        else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {

                            AddToObjectIfNotNull(testObject, "ct", test.CipherText);
                        }




                        AddToObjectIfNotNull(testObject, "iv1", test.IV1);
                        AddToObjectIfNotNull(testObject, "iv2", test.IV2);
                        AddToObjectIfNotNull(testObject, "iv3", test.IV3);

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
        public List<dynamic> ResultProjection
        {
            get
            {
                var tests = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();

                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new Dictionary<string, object>();

                                var keys = new TDESKeys(result.Keys);
                                for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    resultObject.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                                }

                                resultObject.Add("iv1", result.IV1);
                                resultObject.Add("iv2", result.IV2);
                                resultObject.Add("iv3", result.IV3);

                                resultObject.Add("pt", result.PlainText);
                                resultObject.Add("ct", result.CipherText);



                                resultsArray.Add(resultObject);
                            }
                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {


                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                AddToObjectIfNotNull(testObject, "ct", test.CipherText);
                            }

                            if (test.FailureTest)
                            {
                                ((IDictionary<string, object>)testObject).Add("decryptFail", true);
                            }
                            else
                            {
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    AddToObjectIfNotNull(testObject, "pt", test.PlainText);
                                }
                            }


                        }

                        tests.Add(testObject);
                    }
                }
                return tests;
            }
        }

        private void SharedProjectionTestCaseInfo(TestCase test, dynamic testObject)
        {
            throw new NotImplementedException();
            //_dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key", test.Key);
        }

        private void SharedProjectionTestGroupInfo(TestGroup group, dynamic updateObject)
        {
            throw new NotImplementedException();
            //((IDictionary<string, object>)updateObject).Add("direction", group.Function);
            //((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
            //((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
            //((IDictionary<string, object>)updateObject).Add("msgLen", group.MessageLength);
            //((IDictionary<string, object>)updateObject).Add("macLen", group.MacLength);
        }
    }
}
