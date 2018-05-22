using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2.Parsers
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

                bool bitOriented = false;
                bool includeNull = true;
                ModeValues mode = ModeValues.SHA1;
                DigestSizes digestSize = DigestSizes.d160;
                string testType = "";

                int lineItr = 0;
                foreach (var line in lines)
                {
                    lineItr++;

                    var workingLine = line.Trim();
                    if (string.IsNullOrEmpty(workingLine))
                    {
                        continue;
                    }

                    if (workingLine.StartsWith("#"))
                    {
                        mode = ExtractMode(workingLine, mode);
                        digestSize = ExtractDigestSize(workingLine, digestSize);
                        testType = ExtractType(workingLine, testType);
                        bitOriented = ExtractOrientation(workingLine, bitOriented);  
                        continue;
                    }

                    if (workingLine.StartsWith("["))
                    {
                        // There should only be 1 test group per file, but this is a good place to tell us
                        // when to assign properties and create the group.

                        workingLine = workingLine.Replace("[", "").Replace("]", "");

                        currentGroup = new TestGroup
                        {
                            Function = mode,
                            DigestSize = digestSize,
                            BitOriented = bitOriented,
                            IncludeNull = includeNull,      // doesn't matter for validator
                            TestType = testType
                        };
                        groups.Add(currentGroup);
                        continue;
                    }

                    if (currentGroup.TestType.ToLower() == "montecarlo")
                    {
                        if (workingLine.StartsWith("Seed", StringComparison.OrdinalIgnoreCase))
                        {
                            currentTestCase = new TestCase {TestCaseId = 0};
                            currentTestCase.Message = new BitString(workingLine.Split("=".ToCharArray())[1].Trim());
                            currentTestCase.ResultsArray = new List<AlgoArrayResponse>();
                            currentResultArrayPosition = -1;

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
                        currentTestCase.SetResultsArrayString(currentResultArrayPosition, valueParts[0].Trim(), valueParts[1].Trim());
                    }
                    else
                    {
                        if (workingLine.StartsWith("Len", StringComparison.OrdinalIgnoreCase))
                        {
                            var parts = workingLine.Split("=".ToCharArray());
                            int.TryParse(parts[1].Trim(), out currentLength);
                            currentTestCase = new TestCase {TestCaseId = currentCaseNumber};
                            currentGroup.Tests.Add(currentTestCase);
                            currentCaseNumber++;
                            continue;
                        }

                        var valueParts = workingLine.Split("=".ToCharArray());
                        currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim(), currentLength);
                    }
                }
            }

            var testVectorSet = new TestVectorSet
            {
                Algorithm = "SHA",
                TestGroups = groups
            };

            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private ModeValues ExtractMode(string line, ModeValues curVal)
        {
            if (line.Contains("SHA-1"))
            {
                return ModeValues.SHA1;
            }
            else if (line.Contains("SHA-"))
            {
                return ModeValues.SHA2;
            }
            else
            {
                return curVal;
            }
        }

        private DigestSizes ExtractDigestSize(string line, DigestSizes curVal)
        {
            if (line.Contains("SHA-1"))
            {
                return DigestSizes.d160;
            }
            else if (line.Contains("SHA-224 "))
            {
                return DigestSizes.d224;
            }
            else if (line.Contains("SHA-256 "))
            {
                return DigestSizes.d256;
            }
            else if (line.Contains("SHA-384 "))
            {
                return DigestSizes.d384;
            }
            else if (line.Contains("SHA-512 "))
            {
                return DigestSizes.d512;
            }
            else if (line.Contains("SHA-512/224"))
            {
                return DigestSizes.d512t224;
            }
            else if (line.Contains("SHA-512/256"))
            {
                return DigestSizes.d512t256;
            }
            else
            {
                return curVal;
            }
        }

        private string ExtractType(string line, string curVal)
        {
            if (line.Contains("ShortMsg"))
            {
                return "short";
            }

            if (line.Contains("LongMsg"))
            {
                return "long";
            }

            if (line.Contains("Monte"))
            {
                return "montecarlo";
            }

            return curVal;
        }

        private bool ExtractOrientation(string line, bool curVal)
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
                return curVal;
            }
        }
    }
}
