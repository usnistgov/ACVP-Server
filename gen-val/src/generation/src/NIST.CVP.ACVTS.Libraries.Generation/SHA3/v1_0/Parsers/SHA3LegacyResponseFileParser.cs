using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0.Parsers
{
    public class SHA3LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
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

            var groups = new List<TestGroup>();

            var files = Directory.GetFiles(path, "*.rsp");
            foreach (var file in files)
            {
                var lines = new List<string>();
                try
                {
                    lines = File.ReadAllLines(file).ToList();
                }
                catch (Exception ex)
                {
                    return new ParseResponse<TestVectorSet>(ex.Message);
                }

                TestGroup currentGroup = null;
                TestCase currentTestCase = null;
                int currentResultArrayPosition = 0;
                AlgoArrayResponse currentArrayResponse = null;

                int currentCaseNumber = 0;
                int currentLength = 0;

                bool bitOrientedInput = false;
                bool includeNull = true;

                string testType = "";
                string digestSize = "";

                foreach (var line in lines)
                {
                    var workingLine = line.Trim();
                    if (string.IsNullOrEmpty(workingLine))
                    {
                        continue;
                    }

                    if (workingLine.StartsWith("#"))
                    {
                        digestSize = ExtractDigestSize(workingLine, digestSize);
                        testType = ExtractType(workingLine, testType);
                        bitOrientedInput = ExtractOrientation(workingLine, bitOrientedInput);
                        continue;
                    }

                    if (workingLine.StartsWith("["))
                    {
                        currentGroup = new TestGroup
                        {
                            Function = ModeValues.SHA3,
                            DigestSize = ShaAttributes.StringToDigest(digestSize),
                            IncludeNull = includeNull,
                            BitOrientedInput = bitOrientedInput,
                            TestType = testType
                        };
                        groups.Add(currentGroup);
                        continue;
                    }

                    if (currentGroup.TestType.ToLower() == "mct")
                    {
                        if (workingLine.StartsWith("Seed", StringComparison.OrdinalIgnoreCase))
                        {
                            currentTestCase = new TestCase { TestCaseId = currentCaseNumber };
                            currentTestCase.Message = new BitString(workingLine.Split("=".ToCharArray())[1].Trim());
                            currentTestCase.ResultsArray = new List<AlgoArrayResponse>();
                            currentResultArrayPosition = -1;
                            currentCaseNumber++;

                            currentGroup.Tests.Add(currentTestCase);
                            continue;
                        }

                        if (workingLine.StartsWith("COUNT = ", StringComparison.OrdinalIgnoreCase))
                        {
                            currentResultArrayPosition++;
                            currentArrayResponse = new AlgoArrayResponse();
                            currentTestCase.ResultsArray.Add(currentArrayResponse);
                            continue;
                        }

                        var valueParts = workingLine.Split("=".ToCharArray());
                        var returnBool = currentTestCase.SetResultsArrayString(currentResultArrayPosition, valueParts[0].Trim(),
                            valueParts[1].Trim());

                        if (!returnBool)
                        {
                            throw new Exception($"Error setting value for MCT: {valueParts[0]} should be {valueParts[1]}");
                        }
                    }
                    else
                    {
                        if (workingLine.StartsWith("Len = ", StringComparison.OrdinalIgnoreCase))
                        {
                            var parts = workingLine.Split("=".ToCharArray());
                            currentTestCase = new TestCase { TestCaseId = currentCaseNumber };
                            int.TryParse(parts[1].Trim(), out currentLength);
                            currentGroup.Tests.Add(currentTestCase);
                            currentCaseNumber++;
                            continue;
                        }

                        var valueParts = workingLine.Split("=".ToCharArray());

                        // Don't apply the length to digest
                        if (valueParts[0].ToLower().Contains("msg"))
                        {
                            currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim(), currentLength);
                        }
                        else
                        {
                            currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
                        }
                    }
                }
            }

            var testVectorSet = new TestVectorSet
            {
                Algorithm = "SHA3",
                TestGroups = groups
            };

            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private string ExtractDigestSize(string line, string digestSize)
        {
            if (line.Contains("SHA3-224"))
            {
                return "224";
            }
            else if (line.Contains("SHA3-256"))
            {
                return "256";
            }
            else if (line.Contains("SHA3-384"))
            {
                return "384";
            }
            else if (line.Contains("SHA3-512"))
            {
                return "512";
            }
            else
            {
                return digestSize;
            }
        }

        private string ExtractType(string line, string type)
        {
            if (line.Contains("Short"))
            {
                return "aft";
            }
            else if (line.Contains("Long"))
            {
                return "aft";
            }
            else if (line.Contains("Monte"))
            {
                return "mct";
            }
            else
            {
                return type;
            }
        }

        private bool ExtractOrientation(string line, bool bitOriented)
        {
            if (line.Contains("BIT"))
            {
                return true;
            }
            else if (line.Contains("BYTE"))
            {
                return false;
            }
            else
            {
                return bitOriented;
            }
        }
    }
}
