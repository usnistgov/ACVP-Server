using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        // TODO delete this and the projections
        private readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions = 
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty, 
                PrintOptionBitStringEmpty.PrintAsEmptyString
            );
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();

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
        //            var updateDict = ((IDictionary<string, object>) updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("direction", group.Function);
        //            updateDict.Add("testType", group.TestType);
        //            updateDict.Add("ivLen", group.IVLength);
        //            updateDict.Add("ptLen", group.PTLength);
        //            updateDict.Add("aadLen", group.AADLength);
        //            updateDict.Add("tagLen", group.TagLength);
        //            updateDict.Add("keyLen", group.KeyLength);
        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "plainText", test.PlainText);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "cipherText", test.CipherText);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key", test.Key);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "aad", test.AAD);
        //                testDict.Add("deferred", test.Deferred);
        //                //testDict.Add("failureTest", test.FailureTest);
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
        //private List<dynamic> PromptProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>)updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("direction", group.Function);
        //            updateDict.Add("testType", group.TestType);
        //            updateDict.Add("ivLen", group.IVLength);
        //            updateDict.Add("ptLen", group.PTLength);
        //            updateDict.Add("aadLen", group.AADLength);
        //            updateDict.Add("tagLen", group.TagLength);
        //            updateDict.Add("keyLen", group.KeyLength);
        //            var tests = new List<dynamic>();
        //            ((IDictionary<string, object>)updateObject).Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "plainText", test.PlainText);
        //                }
        //                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "cipherText", test.CipherText);
        //                }
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key", test.Key);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV);
        //                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "aad", test.AAD);
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
        //private List<dynamic> ResultProjection
        //{
        //    get
        //    {
        //        var groups = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic groupObject = new ExpandoObject();
        //            var groupDict = (IDictionary<string, object>)groupObject;
        //            groupDict.Add("tgId", group.TestGroupId);

        //            var tests = new List<dynamic>();
        //            groupDict.Add("tests", tests);

        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>)testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "cipherText", test.CipherText);
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV);
        //                }

        //                if (test.FailureTest)
        //                {
        //                    testDict.Add("decryptFail", true);
        //                }
        //                else
        //                {
        //                    if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "plainText", test.PlainText);
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
