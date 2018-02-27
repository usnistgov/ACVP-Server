using Newtonsoft.Json;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestVectorSet : ITestVectorSet
    {
        private readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions =
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty,
                PrintOptionBitStringEmpty.PrintAsEmptyString);

        public string Algorithm { get; set; }
        public string Mode { get; set; } = string.Empty;
        public bool IsSample { get; set; }
        
        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        public TestVectorSet() { }

        public TestVectorSet(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);
                TestGroups.Add(group);
            }
        }

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
                    var updateObject = BuildGroupInformation(group);
                    var updateDict = ((IDictionary<string, object>)updateObject);
                    var tests = new List<dynamic>();
                    
                    updateDict.Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.Equals("MCT", StringComparison.OrdinalIgnoreCase))
                        {
                            var resultsArray = new List<dynamic>();

                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                var resultDict = ((IDictionary<string, object>) resultObject);

                                var keys = new TDESKeys(result.Keys);
                                for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    resultDict.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                                }

                                resultDict.Add("iv1", result.IV1);
                                resultDict.Add("iv2", result.IV2);
                                resultDict.Add("iv3", result.IV3);
                                resultDict.Add("pt", result.PlainText);
                                resultDict.Add("ct", result.CipherText);
                                resultDict.Add("ctLen", result.CipherTextLength);
                                resultDict.Add("ptLen", result.PlainTextLength);

                                resultsArray.Add(resultObject);
                            }
                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key1", test.Key1);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key2", test.Key2);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key3", test.Key3);

                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct", test.CipherText);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct1", test.CipherText1);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct2", test.CipherText2);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct3", test.CipherText3);
                                AddToObjectIfNotNull(testObject, "ctLen", test.CipherTextLength);
                            }

                            else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt", test.PlainText);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt1", test.PlainText1);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt2", test.PlainText2);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt3", test.PlainText3);
                                AddToObjectIfNotNull(testObject, "ptLen", test.PlainTextLength);
                            }

                            testDict.Add("iv1", test.IV1);
                            testDict.Add("iv2", test.IV2);
                            testDict.Add("iv3", test.IV3);
                        }

                        testDict.Add("deferred", test.Deferred);
                        testDict.Add("failureTest", test.FailureTest);
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
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    var updateObject = BuildGroupInformation(group);
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key1", test.Key1);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key2", test.Key2);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key3", test.Key3);

                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt", test.PlainText);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt1", test.PlainText1);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt2", test.PlainText2);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt3", test.PlainText3);
                            AddToObjectIfNotNull(testObject, "ptLen", test.PlainTextLength);
                        }
                        else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct", test.CipherText);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct1", test.CipherText1);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct2", test.CipherText2);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct3", test.CipherText3);
                            AddToObjectIfNotNull(testObject, "ctLen", test.CipherTextLength);
                        }

                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV1);

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
                var groups = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic groupObject = new ExpandoObject();
                    var groupDict = (IDictionary<string, object>) groupObject;
                    groupDict.Add("tgId", group.TestGroupId);

                    var tests = new List<dynamic>();
                    groupDict.Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
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
                                resultObject.Add("ptLen", result.PlainTextLength);
                                resultObject.Add("ct", result.CipherText);
                                resultObject.Add("ctLen", result.CipherTextLength);

                                resultsArray.Add(resultObject);
                            }
                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {

                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct", test.CipherText);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct1", test.CipherText1);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct2", test.CipherText2);
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "ct3", test.CipherText3);
                                AddToObjectIfNotNull(testObject, "ctLen", test.CipherTextLength);
                            }

                            if (test.FailureTest)
                            {
                                testDict.Add("decryptFail", true);
                            }
                            else
                            {
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt", test.PlainText);
                                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt1", test.PlainText1);
                                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt2", test.PlainText2);
                                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "pt3", test.PlainText3);
                                    AddToObjectIfNotNull(testObject, "ptLen", test.PlainTextLength);
                                }
                            }
                        }

                        tests.Add(testObject);
                    }

                    groups.Add(groupObject);
                }

                return groups;
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

        private dynamic BuildGroupInformation(TestGroup group)
        {
            dynamic updateObject = new ExpandoObject();
            var updateDict = ((IDictionary<string, object>)updateObject);
            updateDict.Add("tgId", group.TestGroupId);
            updateDict.Add("direction", group.Function);
            updateDict.Add("testType", group.TestType);
            updateDict.Add("keyingOption", group.KeyingOption);
            return updateObject;
        }
        
        private void AddToObjectIfNotNull(dynamic obj, string propertyName, object propertyValue)
        {
            if (propertyValue != null)
            {
                ((IDictionary<string, object>)obj).Add(propertyName, propertyValue);
            }
        }
    }
}