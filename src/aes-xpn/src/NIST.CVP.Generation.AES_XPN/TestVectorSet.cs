using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestVectorSet: ITestVectorSet<TestGroup, TestCase>
    {
        private readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions = new DynamicBitStringPrintWithOptions(PrintOptionBitStringNull.DoNotPrintProperty, PrintOptionBitStringEmpty.PrintAsEmptyString);

        public string Algorithm { get; set; } = "AES";
        public string Mode { get; set; } = "XPN";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
        
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
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "cipherText", test.CipherText);
        //                    _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "tag", test.Tag);

        //                    if (group.IVGeneration.ToLower() == "internal")
        //                    {
        //                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV);
        //                    }

        //                    if (group.SaltGen.ToLower() == "internal")
        //                    {
        //                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "salt", test.Salt);
        //                    }
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
    }
}
