using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES_CFB1.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet>
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

            var files = Directory.GetFiles(path, "*.fax");
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
                BitOrientedAlgoArrayResponse currentArrayResponse = null;

                int lineIterator = 0;

                foreach (var line in lines)
                {
                    lineIterator++;

                    var workingLine = line.Trim();
                    if (string.IsNullOrEmpty(workingLine))
                    {
                        continue;
                    }
                    if (workingLine.StartsWith("#"))
                    {
                        continue;
                    }
                    if (workingLine.StartsWith("["))
                    {
                        // New test group when "[" encountered
                        workingLine = workingLine.Replace("[", "").Replace("]", "");
                        
                        currentGroup = new TestGroup()
                        {
                            Function = workingLine,
                            KeyLength = 0,
                            TestType = file.Contains("MCT") ? "MCT" : string.Empty
                        };
                        groups.Add(currentGroup);
                        continue;
                    }

                    if (currentGroup.TestType.ToLower() == "mct")
                    {
                        // New test case on count 0
                        if (workingLine.StartsWith("COUNT = 0", StringComparison.OrdinalIgnoreCase))
                        {
                            currentTestCase = new TestCase() { TestCaseId = 0 };
                            currentTestCase.ResultsArray = new List<BitOrientedAlgoArrayResponse>();
                            currentResultArrayPosition = 0;
                            currentArrayResponse = new BitOrientedAlgoArrayResponse();
                            currentTestCase.ResultsArray.Add(currentArrayResponse);

                            currentGroup.Tests.Add(currentTestCase);

                            continue;
                        }

                        // This is the beginning of a resultsArray element
                        if (workingLine.StartsWith("COUNT = ", StringComparison.OrdinalIgnoreCase))
                        {
                            currentResultArrayPosition++;
                            currentArrayResponse = new BitOrientedAlgoArrayResponse();
                            currentTestCase.ResultsArray.Add(currentArrayResponse);
                            continue;
                        }

                        // Set the parts of the test case
                        string[] valueParts = workingLine.Split("=".ToCharArray());
                        currentTestCase.SetResultsArrayString(currentResultArrayPosition, valueParts[0].Trim(), valueParts[1].Trim());
                    }
                    else
                    {
                        if (workingLine.StartsWith("Count", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] parts = workingLine.Split("=".ToCharArray());
                            int caseId = -1;
                            int.TryParse(parts[1].Trim(), out caseId);
                            currentTestCase = new TestCase { TestCaseId = caseId };
                            currentGroup.Tests.Add(currentTestCase);
                            continue;
                        }

                        string[] valueParts = workingLine.Split("=".ToCharArray());
                        currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
                    }

                    
                }
            }

            var testVectorSet = new TestVectorSet { Algorithm = "AES-OFB", TestGroups = groups.Select(g => (ITestGroup)g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}
