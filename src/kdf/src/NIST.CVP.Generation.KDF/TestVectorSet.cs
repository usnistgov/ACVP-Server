using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
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
        //            updateDict.Add("kdfMode", EnumHelpers.GetEnumDescriptionFromEnum(group.KdfMode));
        //            updateDict.Add("macMode", EnumHelpers.GetEnumDescriptionFromEnum(group.MacMode));
        //            updateDict.Add("counterLocation", EnumHelpers.GetEnumDescriptionFromEnum(group.CounterLocation));
        //            updateDict.Add("keyOutLength", group.KeyOutLength);

        //            if (group.KdfMode == KdfModes.Feedback)
        //            {
        //                updateDict.Add("zeroLengthIv", group.ZeroLengthIv);
        //            }

        //            if (group.CounterLocation != CounterLocations.None)
        //            {
        //                updateDict.Add("counterLength", group.CounterLength);
        //            }

        //            var tests = new List<dynamic>();
        //            ((IDictionary<string, object>)updateObject).Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase) t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("keyIn", test.KeyIn);

        //                if (group.KdfMode == KdfModes.Feedback)
        //                {
        //                    testDict.Add("iv", test.IV);
        //                }

        //                testDict.Add("deferred", test.Deferred);

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
        //            var updateDict = ((IDictionary<string, object>) updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("kdfMode", EnumHelpers.GetEnumDescriptionFromEnum(group.KdfMode));
        //            updateDict.Add("macMode", EnumHelpers.GetEnumDescriptionFromEnum(group.MacMode));
        //            updateDict.Add("counterLocation", EnumHelpers.GetEnumDescriptionFromEnum(group.CounterLocation));
        //            updateDict.Add("keyOutLength", group.KeyOutLength);

        //            if (group.KdfMode == KdfModes.Feedback)
        //            {
        //                updateDict.Add("zeroLengthIv", group.ZeroLengthIv);
        //            }

        //            if (group.CounterLocation != CounterLocations.None)
        //            {
        //                updateDict.Add("counterLength", group.CounterLength);
        //            }

        //            var tests = new List<dynamic>();
        //            ((IDictionary<string, object>)updateObject).Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase) t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("keyIn", test.KeyIn);

        //                if (group.KdfMode == KdfModes.Feedback)
        //                {
        //                    testDict.Add("iv", test.IV);
        //                }

        //                testDict.Add("deferred", test.Deferred);

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
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);

        //                if (IsSample)
        //                {
        //                    testDict.Add("keyOut", test.KeyOut);
        //                    testDict.Add("fixedData", test.FixedData);

        //                    if (group.KdfMode == KdfModes.Counter && group.CounterLocation == CounterLocations.MiddleFixedData)
        //                    {
        //                        testDict.Add("breakLocation", test.BreakLocation);
        //                    }
        //                }
                        
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
