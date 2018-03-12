using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3.Parsers
{
    public class SHAKELegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
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

                bool bitOrientedOutput = false;
                bool bitOrientedInput = false;
                bool includeNull = true;

                string testType = "";
                string function = "shake";
                int digestSize = 0;

                int brackets = 0;
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
                        brackets++;
                        if (IsLastBracket(brackets, testType))
                        {
                            currentGroup = new TestGroup
                            {
                                Function = function,
                                DigestSize = digestSize,
                                IncludeNull = includeNull,                  // doesnt matter
                                BitOrientedInput = bitOrientedInput,        // doesnt matter
                                BitOrientedOutput = bitOrientedOutput,      // doesnt matter
                                TestType = testType
                            };
                            groups.Add(currentGroup);
                            brackets = 0;
                        }
                        
                        continue;
                    }

                    if (currentGroup.TestType.ToLower() == "mct")
                    {
                        if (workingLine.StartsWith("Msg", StringComparison.OrdinalIgnoreCase))
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

                        // Ignore the output length, it is always a multiple of 8 so we can get the length from the normal hex
                        if (workingLine.StartsWith("Output = ", StringComparison.OrdinalIgnoreCase))
                        {
                            var valueParts = workingLine.Split("=".ToCharArray());
                            var returnBool = currentTestCase.SetResultsArrayString(currentResultArrayPosition, valueParts[0].Trim(),
                                valueParts[1].Trim());

                            if (!returnBool)
                            {
                                throw new Exception($"Error setting value for MCT: {valueParts[0]} should be {valueParts[1]}");
                            }
                        }
                    }
                    else if (currentGroup.TestType.ToLower() == "vot")
                    {
                        if (workingLine.StartsWith("COUNT = ", StringComparison.OrdinalIgnoreCase))
                        {
                            currentTestCase = new TestCase {TestCaseId = currentCaseNumber};
                            currentGroup.Tests.Add(currentTestCase);
                            currentCaseNumber++;
                            continue;
                        }

                        if(workingLine.StartsWith("Outputlen = ", StringComparison.OrdinalIgnoreCase))
                        {
                            var parts = workingLine.Split("=".ToCharArray());
                            int.TryParse(parts[1].Trim(), out currentLength);
                            continue;
                        }

                        if (workingLine.StartsWith("Msg = ", StringComparison.OrdinalIgnoreCase))
                        {
                            var valueParts = workingLine.Split("=".ToCharArray());
                            currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
                            continue;
                        }

                        if (workingLine.StartsWith("Output = ", StringComparison.OrdinalIgnoreCase))
                        {
                            var valueParts = workingLine.Split("=".ToCharArray());
                            currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim(), currentLength);
                            continue;
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
                Algorithm = "SHAKE",
                TestGroups = groups
            };

            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private int ExtractDigestSize(string line, int digestSize)
        {
            if (line.Contains("SHAKE128"))
            {
                return 128;
            }
            else if (line.Contains("SHAKE256"))
            {
                return 256;
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
            else if (line.Contains("Variable"))
            {
                return "vot";
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

        private bool IsLastBracket(int brackets, string testType)
        {
            if (testType == "aft" && brackets == 1)
            {
                return true;
            }

            if (testType == "mct" && brackets == 2)
            {
                return true;
            }

            if (testType == "vot" && brackets == 4)
            {
                return true;
            }

            return false;
        }
    }
}
