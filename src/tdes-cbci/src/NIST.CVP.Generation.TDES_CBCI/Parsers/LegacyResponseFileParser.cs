using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CBCI.Parsers
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

            var testVectorSet = new TestVectorSet
            {
                Algorithm = "TDES-CBCI"
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

                //in some cases, the input plain/cipher text needs to be expanded into three blocks
                var textsToExpand = new[] {"InversePermutation", "SubstitutionTable", "VariableKey", "VariableText", "Permutation"};
                foreach (Match match in encryptDecryptRegEx.Matches(fileContent))
                {
                    var testGroup = GetTestGroupFromFileName(file);
                    var tests = CreateTestCases(match.Groups["tests"].Value, testGroup.TestType == "MCT", textsToExpand.Any(x => x == testGroup.TestType));

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

        private List<TestCase> CreateTestCases(string input, bool isMCT, bool expandText)
        {
            var testCases = new List<TestCase>();
            var resultsArray = new List<AlgoArrayResponseWithIvs>();

            var regExTestCase = new Regex(@"(?:[A-Za-z0-9]* = [0-9a-f]*\n)*\n");
            var regExProperty = new Regex(@"(?<prop>[A-Za-z0-9]*) = (?<val>[0-9a-f]*)");


            input = input.Trim();
            //add a pair of new lines to ensure that the last case is matched
            input = input + "\n\n";

            //the .where clause will filter out empty matches
            var matches = regExTestCase.Matches(input).Where(x => x.Value.Trim() != "");

            foreach (Match match in matches) 
            {
                //create bitstring for all the non-null properties
                var properties = regExProperty.Matches(match.Value.Trim());
                var propertiesMap = new Dictionary<string, BitString>()
                {
                    { "KEYs", null},
                    { "KEY1", null},
                    { "KEY2", null},
                    { "KEY3", null},
                    { "IV1", null},
                    { "IV2", null},
                    { "IV3", null},
                    { "PLAINTEXT", null},
                    { "PLAINTEXT1", null},
                    { "PLAINTEXT2", null},
                    { "PLAINTEXT3", null},
                    { "CIPHERTEXT", null},
                    { "CIPHERTEXT1", null},
                    { "CIPHERTEXT2", null},
                    { "CIPHERTEXT3", null}
                };
                foreach (Match property in properties)
                {
                    var prop = property.Groups["prop"].Value;
                    var val = property.Groups["val"].Value;
                    if (prop != "COUNT")
                    {
                        propertiesMap[prop] = new BitString(val);
                    }
                }

                if (propertiesMap["KEYs"] == null)
                {
                    propertiesMap["KEYs"] = propertiesMap["KEY1"]
                        .ConcatenateBits(propertiesMap["KEY2"])
                        .ConcatenateBits(propertiesMap["KEY3"]);
                }
                //If only PLAINTEXT1 or CIPHERTEXT1 are present, the TEXT will just be repeated 3 times
                BitString pt, ct;
                if (propertiesMap["PLAINTEXT"] != null)
                {
                    pt = propertiesMap["PLAINTEXT"];
                }
                else if (propertiesMap["PLAINTEXT2"] != null)
                {
                    pt = propertiesMap["PLAINTEXT1"].ConcatenateBits(propertiesMap["PLAINTEXT2"]).ConcatenateBits(propertiesMap["PLAINTEXT3"]);
                }
                else
                {
                    pt = propertiesMap["PLAINTEXT1"].ConcatenateBits(propertiesMap["PLAINTEXT1"]).ConcatenateBits(propertiesMap["PLAINTEXT1"]);
                }

                if (propertiesMap["CIPHERTEXT"] != null)
                {
                    ct = propertiesMap["CIPHERTEXT"];
                }
                else if (propertiesMap["CIPHERTEXT2"] != null)
                {
                    ct = propertiesMap["CIPHERTEXT1"].ConcatenateBits(propertiesMap["CIPHERTEXT2"]).ConcatenateBits(propertiesMap["CIPHERTEXT3"]);
                }
                else
                {
                    ct = propertiesMap["CIPHERTEXT1"].ConcatenateBits(propertiesMap["CIPHERTEXT1"]).ConcatenateBits(propertiesMap["CIPHERTEXT1"]);
                }

                if (expandText)
                {
                    if (pt.BitLength == 64)
                    {
                        pt = pt.ConcatenateBits(pt).ConcatenateBits(pt);
                    }
                    if (ct.BitLength == 64)
                    {
                        ct = ct.ConcatenateBits(ct).ConcatenateBits(ct);
                    }
                }

                if (pt.BitLength % 192 != 0)
                {
                    throw new ArgumentException("Invalud plain text size");
                }
                if (ct.BitLength % 192 != 0)
                {
                    throw new ArgumentException("Invalud cipher text size");
                }
                if (isMCT)
                {
                    resultsArray.Add(new AlgoArrayResponseWithIvs()
                    {
                        Keys = propertiesMap["KEYs"],
                        IV1 = propertiesMap["IV1"],
                        IV2 = propertiesMap["IV2"],
                        IV3 = propertiesMap["IV3"],
                        PlainText = pt,
                        CipherText = ct
                    });
                }
                else
                {
                    testCases.Add(new TestCase()
                    {
                        Keys = propertiesMap["KEYs"],
                        IV1 = propertiesMap["IV1"],
                        IV2 = propertiesMap["IV2"],
                        IV3 = propertiesMap["IV3"],
                        PlainText =  pt,
                        CipherText = ct,
                    });

                }

            }
            return isMCT ?
                new List<TestCase> { new TestCase { ResultsArray = resultsArray } } :
                testCases;
        }

        private TestGroup GetTestGroupFromFileName(string file)
        {
            var fileNameRegex = new Regex(@"(?:TCBCI)(?<type>[a-zA-Z]*)(?<keyingOption>\d)?(?:\.fax)");

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

    }
}
