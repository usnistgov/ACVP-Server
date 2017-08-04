using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public TestVectorSet()
        {
        }

        public TestVectorSet(dynamic answers, dynamic prompts)
        {
            SetAnswerAndPrompts(answers, prompts);
        }

        public override List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        //_bitStringPrinter.AddToDynamic(testObject, "key", test.Key);
                        _bitStringPrinter.AddToDynamic(testObject, "key1", test.Key1);
                        _bitStringPrinter.AddToDynamic(testObject, "key2", test.Key2);
                        _bitStringPrinter.AddToDynamic(testObject, "key3", test.Key3);

                        if (group.Direction.ToLower() == "encrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }
                        if (group.Direction.ToLower() == "decrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "plainText", test.PlainText);
                        }

                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        public override List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        //_bitStringPrinter.AddToDynamic(testObject, "key", test.Key);
                        _bitStringPrinter.AddToDynamic(testObject, "key1", test.Key1);
                        _bitStringPrinter.AddToDynamic(testObject, "key2", test.Key2);
                        _bitStringPrinter.AddToDynamic(testObject, "key3", test.Key3);

                        if (group.Direction.ToLower() == "encrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "plainText", test.PlainText);
                        }
                        if (group.Direction.ToLower() == "decrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        public override List<dynamic> ResultProjection
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

                        if (group.Direction.ToLower() == "encrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }

                        if (test.FailureTest)
                        {
                            ((IDictionary<string, object>)testObject).Add("decryptFail", true);
                        }
                        else
                        {
                            if (group.Direction.ToLower() == "decrypt")
                            {
                                _bitStringPrinter.AddToDynamic(testObject, "plainText", test.PlainText);
                            }
                        }

                        tests.Add(testObject);
                    }
                }
                return tests;
            }
        }
    }
}
