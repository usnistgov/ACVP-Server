using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
    {
        public ParseResponse<TestVectorSet> Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new ParseResponse<TestVectorSet>("There was no path supplied.");
            }

            if (!Directory.Exists(path))
            {
                return new ParseResponse<TestVectorSet>($"Could not find path {path}");
            }

            var algo = EnumHelpers.GetEnumFromEnumDescription<AlgoMode>(Path.GetFileName(path));

            // these are the only algorithms this parse will support

            if (!new[] { AlgoMode.TDES_CFB1_v1_0, AlgoMode.TDES_CFB8_v1_0, AlgoMode.TDES_CFB64_v1_0 }.Contains(algo))
            {
                throw new ArgumentException("Unsupported algorithm.");
            }

            var testVectorSet = new TestVectorSet
            {
                Algorithm = EnumHelpers.GetEnumDescriptionFromEnum(algo),
            };

            // this regex will allow to pull out just the encryption group, or just the decryption group

            var encryptDecryptRegEx = new Regex(@"\[(?<direction>[a-zA-Z]*)\](?<tests>[\sa-zA-Z0-9 =]*)");


            var groups = new List<TestGroup>();


            var files = Directory.GetFiles(path, "*.fax");
            foreach (var file in files)
            {

                string fileContent;
                try
                {
                    fileContent = File.ReadAllText(file).Replace("\r\n", "\n");
                }
                catch (Exception ex)
                {
                    return new ParseResponse<TestVectorSet>(ex.Message);
                }
                foreach (Match match in encryptDecryptRegEx.Matches(fileContent))
                {
                    var testGroup = GetTestGroupFromFileName(file);
                    var tests = CreateTestCases(match.Groups["tests"].Value, algo != AlgoMode.TDES_CFB1_v1_0, testGroup.TestType == "MCT");

                    if (tests.Any())
                    {
                        testGroup.Function = match.Groups["direction"].Value.ToLower();
                        testGroup.Tests = tests;
                        groups.Add(testGroup);
                    }
                }
            }
            testVectorSet.TestGroups = groups;
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private List<TestCase> CreateTestCases(string input, bool isPtAndCtHex, bool isMCT)
        {
            var testCases = new List<TestCase>();
            var resultsArray = new List<AlgoArrayResponse>();

            var testCaseRegex = new Regex(@"COUNT = (?<count>\d*)(?:\n" +
                    @"(?:KEY1 = (?<key1>[0-9acbdefABCDEF]*)\n" +
                    @"KEY2 = (?<key2>[0-9acbdefABCDEF]*)\n" +
                    @"KEY3 = (?<key3>[0-9acbdefABCDEF]*))|\n" +
                    @"(?:KEYs = (?<keys>[0-9acbdefABCDEF]*)))\n" +
                    @"IV = (?<iv>[0-9acbdefABCDEF]*)(?:(?:\n" +
                    @"PLAINTEXT = (?<pt>[0-9acbdefABCDEF]*)\n" +
                    @"CIPHERTEXT = (?<ct>[0-9acbdefABCDEF]*))|(?:\n" +
                    @"CIPHERTEXT = (?<ct1>[0-9acbdefABCDEF]*)\n" +
                    @"PLAINTEXT = (?<pt1>[0-9acbdefABCDEF]*)))");

            var matches = testCaseRegex.Matches(input);

            foreach (Match match in matches)
            {
                var iv = new BitString(match.Groups["iv"].Value);
                var key1 = new BitString(match.Groups["key1"].Value + match.Groups["keys"].Value);
                var key2 = new BitString(match.Groups["key2"].Value + match.Groups["keys"].Value);
                var key3 = new BitString(match.Groups["key3"].Value + match.Groups["keys"].Value);
                BitString ct;
                BitString pt;

                var ptStr = match.Groups["pt"].Value + match.Groups["pt1"].Value;
                var ctStr = match.Groups["ct"].Value + match.Groups["ct1"].Value;

                if (isPtAndCtHex)
                {
                    ct = new BitString(ctStr);
                    pt = new BitString(ptStr);
                }
                else
                {
                    ct = new BitString(new BitArray(ctStr.Reverse().Select(x => x == '1' || x == '0' ?
                        x == '1' :
                        throw new InvalidCastException()).ToArray()));

                    pt = new BitString(new BitArray(ptStr.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                        x == '1' :
                        throw new InvalidCastException()).ToArray()));
                }

                if (isMCT)
                {
                    resultsArray.Add(new AlgoArrayResponse()
                    {
                        CipherText = ct,
                        IV = iv,
                        PlainText = pt,
                        Key1 = key1,
                        Key2 = key2,
                        Key3 = key3
                    });
                }
                else
                {
                    testCases.Add(new TestCase()
                    {
                        CipherText = ct,
                        Iv = iv,
                        PlainText = pt,
                        Key1 = key1,
                        Key2 = key2,
                        Key3 = key3
                    });

                }

            }
            return isMCT ?
                new List<TestCase> { new TestCase { ResultsArray = resultsArray } } :
                testCases;
        }

        private TestGroup GetTestGroupFromFileName(string file)
        {
            var fileNameRegex = new Regex(@"(?:TCFB\d\d?)(?<type>[a-zA-Z]*)(?<keyingOption>\d)?(?:\.fax)");

            var fileNameToTestType = new Dictionary<string, string>()
            {
                {"invperm", "InversePermutation"},
                {"MMT", "MultiBlockMessage"},
                {"Monte", "MCT"},
                {"permop", "Permutation"},
                {"subtab", "SubstitutionTable"},
                {"varkey", "VariableKey"},
                {"vartext", "VariableText"}
            };

            var match = fileNameRegex.Matches(file)[0];
            var testType = fileNameToTestType[match.Groups["type"].Value];
            var keyingOption = match.Groups["keyingOption"].Value;

            var testGroup = new TestGroup
            {
                TestType = testType,
                KeyingOption = keyingOption != "" ? TdesHelpers.GetKeyingOptionFromNumberOfKeys(int.Parse(keyingOption)) : 1
            };

            return testGroup;
        }


        //protected TestGroup CreateTestGroup(string path)
        //{
        //    var testGroup = new TestGroup();
        //    var fileName = Path.GetFileName(path).ToLower();

        //    //parse direction
        //    if (fileName.Contains("gen"))
        //    {
        //        testGroup.Function = "gen";
        //    }
        //    else if (fileName.Contains("ver"))
        //    {
        //        testGroup.Function = "ver";
        //    }
        //    else
        //    {
        //        throw new Exception("Could not determine direction from filename");
        //    }

        //    //parse key size
        //    if (fileName.Contains("128"))
        //    {
        //        testGroup.KeyLength = 128;
        //        testGroup.CmacType = CmacTypes.AES128;
        //    }
        //    else if (fileName.Contains("192"))
        //    {
        //        testGroup.KeyLength = 192;
        //        testGroup.CmacType = CmacTypes.AES192;

        //    }
        //    else if (fileName.Contains("256"))
        //    {
        //        testGroup.KeyLength = 256;
        //        testGroup.CmacType = CmacTypes.AES256;
        //    }
        //    else
        //    {
        //        throw new Exception("Could not determine keysize from filename.");
        //    }
        //    testGroup.Tests = new List<ITestCase>();
        //    return testGroup;
        //}
    }
}
