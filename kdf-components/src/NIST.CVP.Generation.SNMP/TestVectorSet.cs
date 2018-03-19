using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SNMP
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();

        //public List<dynamic> AnswerProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup) g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>) updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("engineId", group.EngineId);
        //            updateDict.Add("passwordLength", group.PasswordLength);

        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase) t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("password", test.Password);
        //                testDict.Add("sharedKey", test.SharedKey);

        //                tests.Add(testObject);
        //            }

        //            list.Add(updateObject);
        //        }

        //        return list;
        //    }
        //}

        //[JsonProperty(PropertyName = "testGroups")]
        //public List<dynamic> PromptProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup) g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            ((IDictionary<string, object>)updateObject).Add("tgId", group.TestGroupId);
        //            ((IDictionary<string, object>)updateObject).Add("engineId", group.EngineId);
        //            ((IDictionary<string, object>)updateObject).Add("passwordLength", group.PasswordLength);

        //            var tests = new List<dynamic>();
        //            ((IDictionary<string, object>)updateObject).Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase) t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
        //                ((IDictionary<string, object>)testObject).Add("password", test.Password);

        //                tests.Add(testObject);
        //            }

        //            list.Add(updateObject);
        //        }

        //        return list;
        //    }
        //}

        //[JsonProperty(PropertyName = "testResults")]
        //public List<dynamic> ResultProjection
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
        //                ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
        //                ((IDictionary<string, object>)testObject).Add("sharedKey", test.SharedKey);

        //                tests.Add(testObject);
        //            }

        //            groups.Add(groupObject);
        //        }

        //        return groups;
        //    }
        //}

        //public dynamic ToDynamic()
        //{
        //    dynamic vectorSetObject = new ExpandoObject();
        //    ((IDictionary<string, object>)vectorSetObject).Add("answerProjection", AnswerProjection);
        //    ((IDictionary<string, object>)vectorSetObject).Add("testGroups", PromptProjection);
        //    ((IDictionary<string, object>)vectorSetObject).Add("resultProjection", ResultProjection);
        //    return vectorSetObject;
        //}
    }
}
