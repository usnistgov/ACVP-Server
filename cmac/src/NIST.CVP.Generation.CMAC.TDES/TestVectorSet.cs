using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public override string Algorithm { get; set; } = "CMAC";
        public override string Mode { get; set; } = "TDES";


        ///// <summary>
        ///// Expected answers (not sent to client)
        ///// </summary>
        //private List<dynamic> AnswerProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>)updateObject);

        //            SharedProjectionTestGroupInfo(group, updateObject);

        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>)testObject);
        //                testDict.Add("tcId", test.TestCaseId);

        //                SharedProjectionTestCaseInfo(test, testObject);

        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "msg", test.Message);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "mac", test.Mac);
        //                if (test.Result != null)
        //                {
        //                    testDict.Add("result", test.Result);
        //                }
        //                testDict.Add("deferred", test.Deferred);
        //                testDict.Add("failureTest", test.FailureTest);

        //                tests.Add(testObject);
        //            }
        //            list.Add(updateObject);

        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// What the client receives (should not include expected answers)
        ///// </summary>
        //[JsonProperty(PropertyName = "testGroups")]
        //private List<dynamic> PromptProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>)updateObject);

        //            SharedProjectionTestGroupInfo(group, updateObject);

        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>)testObject);
        //                testDict.Add("tcId", test.TestCaseId);

        //                SharedProjectionTestCaseInfo(test, testObject);

        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "msg", test.Message);

        //                if (group.Function.Equals("ver", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "mac", test.Mac);
        //                }

        //                tests.Add(testObject);
        //            }
        //            list.Add(updateObject);
        //        }

        //        return list;
        //    }
        //}

        ///// <summary>
        ///// Debug projection (internal), as well as potentially sample projection (sent to client)
        ///// </summary>
        //[JsonProperty(PropertyName = "testResults")]
        //private List<dynamic> ResultProjection
        //{
        //    get
        //    {
        //        var groups = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic groupObject = new ExpandoObject();
        //            var groupDict = (IDictionary<string, object>) groupObject;
        //            groupDict.Add("tgId", group.TestGroupId);

        //            var tests = new List<dynamic>();
        //            groupDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>)testObject);
        //                testDict.Add("tcId", test.TestCaseId);

        //                if (group.Function.Equals("gen", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "mac", test.Mac);
        //                }

        //                if (group.Function.Equals("ver", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    testDict.Add("result", test.Result);
        //                }

        //                tests.Add(testObject);
        //            }

        //            groups.Add(groupObject);
        //        }

        //        return groups;
        //    }
        //}

        //private void SharedProjectionTestGroupInfo(TestGroup group, dynamic updateObject)
        //{
        //    var updateDict = ((IDictionary<string, object>)updateObject);
        //    updateDict.Add("tgId", group.TestGroupId);
        //    updateDict.Add("direction", group.Function);
        //    updateDict.Add("testType", group.TestType);
        //    updateDict.Add("keyingOption", group.KeyingOption);
        //    updateDict.Add("msgLen", group.MessageLength);
        //    updateDict.Add("macLen", group.MacLength);
        //}

        //private void SharedProjectionTestCaseInfo(TestCase test, dynamic testObject)
        //{
        //    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key1", test.Key1);
        //    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key2", test.Key2);
        //    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key3", test.Key3);
        //}
    }
}
