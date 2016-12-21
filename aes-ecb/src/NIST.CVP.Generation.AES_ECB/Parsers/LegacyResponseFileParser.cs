using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES_ECB.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet>
    {
        public ParseResponse<TestVectorSet> Parse(string path)
        {
            // Gracefully handle errors
            if (string.IsNullOrEmpty(path))
            {
                return new ParseResponse<TestVectorSet>("There was no path supplied.");
            }

            if (!File.Exists(path))
            {
                return new ParseResponse<TestVectorSet>($"Could not find file: {path}");
            }

            List<string> lines = new List<string>();
            try
            {
                lines = File.ReadAllLines(path).ToList();
            }catch(Exception e)
            {
                return new ParseResponse<TestVectorSet>(e.Message);
            }

            string fileName = Path.GetFileName(path).ToLower();
            string direction = "";

            List<TestGroup> groups = new List<TestGroup>();
            TestGroup currentGroup = null;
            TestCase currentTestCase = null;
            int keyLen = -1;
            int ptLen = -1;

            foreach(string line in lines)
            {
                string workingLine = line.Trim();

                // Ignore blank lines
                if(string.IsNullOrEmpty(workingLine))
                {
                    continue;
                }

                // Ignore header, but grab key length
                if (workingLine.StartsWith("#"))
                {
                    if(keyLen == -1 && workingLine.Contains("Key Length"))
                    {
                        string[] parts = workingLine.Split(":".ToCharArray());
                        int.TryParse(parts[1].Trim(), out keyLen);
                    }
                    continue;
                }

                // Determine direction, always marks start of new TestGroup
                if (workingLine.StartsWith("["))
                {
                    direction = workingLine.Replace("[", "").Replace("]", "").ToLower();
                    currentGroup = new TestGroup { Function = direction };
                    currentGroup.SetString("keylen", keyLen.ToString());
                    groups.Add(currentGroup);

                    // Refresh plaintext length for the new TestGroup
                    ptLen = -1;

                    continue;
                }

                // Build TestCases
                if (workingLine.ToLower().StartsWith("count"))
                {
                    string[] parts = workingLine.Split("=".ToCharArray());
                    int caseId = -1;
                    int.TryParse(parts[1].Trim(), out caseId);
                    currentTestCase = new TestCase { TestCaseId = caseId };
                    currentGroup.Tests.Add(currentTestCase);
                    continue;
                }

                // Add to each TestCase
                string[] valueParts = workingLine.Split("=".ToCharArray());
                currentTestCase.SetString(valueParts[0].Trim().ToLower(), valueParts[1].Trim().ToLower());

                // Initial set for plaintext length
                if (ptLen == -1 && valueParts[0] == "plaintext")
                {
                    ptLen = valueParts[1].Length * 4;
                    currentGroup.SetString("ptlen", ptLen.ToString());
                }
            }

            TestVectorSet testVectorSet = new TestVectorSet { Algorithm = "AES-ECB", TestGroups = groups.Select(g => (ITestGroup)g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}
