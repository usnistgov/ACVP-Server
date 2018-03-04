using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.TDES_ECB.Parsers
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
                AlgoArrayResponse currentArrayResponse = null;
                List<string> keyParts = new List<string>();

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
                            //If the file name contains "Monte", TestType is "MCT"; if file name contains "MMT", TestType is "MMT", otherwise it is an empty String.
                            TestType = file.Contains("Monte") ? "MCT" : file.Contains("MMT") ? "MMT" : string.Empty
                        };
                        groups.Add(currentGroup);
                        continue;
                    }

                    if (currentGroup.TestType.ToLower() == "mct")
                    {
                        // New test case on count 0
                        if (workingLine.StartsWith("COUNT = 0", StringComparison.OrdinalIgnoreCase))
                        {
                            currentTestCase = new TestCase() {TestCaseId = 0};
                            currentTestCase.ResultsArray = new List<AlgoArrayResponse>();
                            currentResultArrayPosition = 0;
                            currentArrayResponse = new AlgoArrayResponse();
                            currentTestCase.ResultsArray.Add(currentArrayResponse);

                            currentGroup.Tests.Add(currentTestCase);

                            continue;
                        }

                        //This is the beginning of a resultsArray element
                        if (workingLine.StartsWith("COUNT = ", StringComparison.OrdinalIgnoreCase))
                        {
                            currentResultArrayPosition++;
                            currentArrayResponse = new AlgoArrayResponse();
                            currentTestCase.ResultsArray.Add(currentArrayResponse);
                            continue;
                        }

                        // Start of keys in MCT
                        if (workingLine.StartsWith("key1 = ", StringComparison.OrdinalIgnoreCase))
                        {
                            keyParts = new List<string>();
                            string[] key = workingLine.Split("=".ToCharArray());
                            keyParts.Add(key[1].Trim());
                        }

                        // Key 2
                        if (workingLine.StartsWith("key2 = ", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] key = workingLine.Split("=".ToCharArray());
                            keyParts.Add(key[1].Trim());
                        }

                        // Final key, create TDESKeys
                        if (workingLine.StartsWith("key3 = ", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] key = workingLine.Split("=".ToCharArray());
                            keyParts.Add(key[1].Trim());

                            StringBuilder sb = new StringBuilder();
                            foreach (string k in keyParts)
                            {
                                sb.Append(k);
                            }

                            currentTestCase.SetResultsArrayString(currentResultArrayPosition, "key", sb.ToString());
                            continue;
                        }

                        // Set the parts of the test case
                        string[] valueParts = workingLine.Split("=".ToCharArray());
                        currentTestCase.SetResultsArrayString(currentResultArrayPosition, valueParts[0].Trim(),
                            valueParts[1].Trim());
                    }
                    else
                    {
                        if (workingLine.StartsWith("Count", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] parts = workingLine.Split("=".ToCharArray());
                            int caseId = -1;
                            int.TryParse(parts[1].Trim(), out caseId);
                            currentTestCase = new TestCase {TestCaseId = caseId};
                            currentGroup.Tests.Add(currentTestCase);
                            continue;
                        }

                        string[] valueParts = workingLine.Split("=".ToCharArray());
                        currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
                    }
                }
            }

            var testVectorSet = new TestVectorSet { Algorithm = "TDESECB", TestGroups = groups.Select(g => (ITestGroup)g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}
