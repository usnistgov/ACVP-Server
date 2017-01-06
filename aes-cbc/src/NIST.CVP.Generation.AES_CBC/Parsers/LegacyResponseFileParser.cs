using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES_CBC.Parsers
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

            var files = Directory.GetFiles(path, "*.rsp");
            foreach (var file in files)
            {
                // @@@ TODO if MCT (Monte Carlo Test) is implemented, stop excluding the MCT files
                if (file.Contains("MCT"))
                {
                    continue;
                }

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

                foreach (var line in lines)
                {
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
                            PTLength = 0
                        };
                        groups.Add(currentGroup);
                        continue;
                    }

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

            var testVectorSet = new TestVectorSet { Algorithm = "AES-CBC", TestGroups = groups.Select(g => (ITestGroup)g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}
