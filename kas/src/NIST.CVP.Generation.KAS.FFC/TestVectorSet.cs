using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase>
    {
        public override List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();

                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    ExpandoObject updateObject = new ExpandoObject();

                    SharedProjectionTestGroupInfo(group, updateObject);

                    var tests = new List<dynamic>();
                    updateObject.TryAdd("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);

                        if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                            testObject.AddBigIntegerWhenNotZero("xStaticServer", test.StaticPrivateKeyServer);
                            testObject.AddBigIntegerWhenNotZero("yStaticServer", test.StaticPublicKeyServer);

                            testObject.AddBigIntegerWhenNotZero("xEphemeralServer", test.EphemeralPrivateKeyServer);
                            testObject.AddBigIntegerWhenNotZero("yEphemeralServer", test.EphemeralPublicKeyServer);

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);
                        }

                        if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            testObject.AddBigIntegerWhenNotZero("xStaticServer", test.StaticPrivateKeyServer);
                            testObject.AddBigIntegerWhenNotZero("yStaticServer", test.StaticPublicKeyServer);
                            testObject.AddBigIntegerWhenNotZero("xEphemeralServer", test.EphemeralPrivateKeyServer);
                            testObject.AddBigIntegerWhenNotZero("yEphemeralServer", test.EphemeralPublicKeyServer);

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                            testObject.AddBigIntegerWhenNotZero("xStaticIut", test.StaticPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("yStaticIut", test.StaticPublicKeyIut);
                            testObject.AddBigIntegerWhenNotZero("xEphemeralIut", test.EphemeralPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("yEphemeralIut", test.EphemeralPublicKeyIut);

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "z", test.Z);
                            if (group.MacType != KeyAgreementMacType.None)
                            {
                                testObject.AddIntegerWhenNotZero("oiLen", test.OiLen);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "otherInfo", test.OtherInfo);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "dkm", test.Dkm);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceAesCcm", test.NonceAesCcm);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "macData", test.MacData);

                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "tagServer", test.Tag);
                            }
                            else
                            {
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "hashZServer", test.HashZ);
                            }
                        }

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
                    ExpandoObject updateObject = new ExpandoObject();

                    SharedProjectionTestGroupInfo(group, updateObject);

                    var tests = new List<dynamic>();
                    updateObject.TryAdd("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);

                        if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                        {
                            testObject.AddBigIntegerWhenNotZero("yStaticServer", test.StaticPublicKeyServer);
                            testObject.AddBigIntegerWhenNotZero("yEphemeralServer", test.EphemeralPublicKeyServer);

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);
                        }

                        if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            testObject.AddBigIntegerWhenNotZero("yStaticServer", test.StaticPublicKeyServer);
                            testObject.AddBigIntegerWhenNotZero("yEphemeralServer", test.EphemeralPublicKeyServer);

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                            testObject.AddBigIntegerWhenNotZero("xStaticIut", test.StaticPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("yStaticIut", test.StaticPublicKeyIut);
                            testObject.AddBigIntegerWhenNotZero("xEphemeralIut", test.EphemeralPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("yEphemeralIut", test.EphemeralPublicKeyIut);

                            if (group.MacType != KeyAgreementMacType.None)
                            {
                                testObject.AddIntegerWhenNotZero("oiLen", test.OiLen);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "otherInfo", test.OtherInfo);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceAesCcm", test.NonceAesCcm);
                                
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "tagIut", test.Tag);
                            }
                            else
                            {
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "hashZIut", test.HashZ);
                            }
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
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);

                        // AFT tests are deferred, and nothing can be projected as a result.

                        if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>) testObject).Add("result",
                                test.FailureTest ? "fail" : "pass");
                        }

                        list.Add(testObject);
                    }
                }
                return list;
            }
        }

        private void SharedProjectionTestGroupInfo(TestGroup @group, dynamic updateObject)
        {
            ((IDictionary<string, object>) updateObject).Add("scheme", EnumHelpers.GetEnumDescriptionFromEnum(group.Scheme));
            ((IDictionary<string, object>) updateObject).Add("testType", group.TestType);
            ((IDictionary<string, object>) updateObject).Add("kasRole", EnumHelpers.GetEnumDescriptionFromEnum(group.KasRole));
            ((IDictionary<string, object>) updateObject).Add("kasMode", EnumHelpers.GetEnumDescriptionFromEnum(group.KasMode));
            ((IDictionary<string, object>) updateObject).Add("parmSet", EnumHelpers.GetEnumDescriptionFromEnum(group.ParmSet));
            ((IDictionary<string, object>) updateObject).Add("hashAlg", ShaAttributes.GetShaAttributes(group.HashAlg.Mode, group.HashAlg.DigestSize).name);

            if (group.MacType != KeyAgreementMacType.None)
            {
                ((IDictionary<string, object>)updateObject).Add("macType", EnumHelpers.GetEnumDescriptionFromEnum(group.MacType));
                ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLen);

                if (group.MacType == KeyAgreementMacType.AesCcm)
                {
                    ((IDictionary<string, object>)updateObject).Add("aesCcmNonceLen", group.AesCcmNonceLen);
                }

                ((IDictionary<string, object>)updateObject).Add("macLen", group.MacLen);
                ((IDictionary<string, object>)updateObject).Add("kdfType", group.KdfType);
                ((IDictionary<string, object>)updateObject).Add("idServerLen", group.IdServerLen);
                DynamicBitStringPrintWithOptions.AddToDynamic(updateObject, "idServer", group.IdServer);

                if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                {
                    ((IDictionary<string, object>)updateObject).Add("idIutLen", group.IdIutLen);
                    DynamicBitStringPrintWithOptions.AddToDynamic(updateObject, "idIut", group.IdIut);
                }
            }

            ((IDictionary<string, object>)updateObject).Add("p", group.P);
            ((IDictionary<string, object>)updateObject).Add("q", group.Q);
            ((IDictionary<string, object>)updateObject).Add("g", group.G);

        }
    }
}