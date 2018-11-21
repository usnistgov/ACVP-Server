using NIST.CVP.Generation.Core.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XTS.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
    {
        public ParseResponse<TestVectorSet> Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new ParseResponse<TestVectorSet>("There was no path supplied.");
            }

            if (!File.Exists(path))
            {
                return new ParseResponse<TestVectorSet>($"Could not find file: {path}");
            }

            var lines = new List<string>();
            try
            {
                lines = File.ReadAllLines(path).ToList();
            }
            catch (Exception ex)
            {
                return new ParseResponse<TestVectorSet>(ex.Message);
            }

            string fileName = Path.GetFileName(path).ToLower();

            var groups = new List<TestGroup>();
            TestGroup currentGroup = null;
            TestCase currentTestCase = null;
            bool inCases = false;
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
                    if (currentGroup == null || inCases)
                    {
                        inCases = false;
                        currentGroup = new TestGroup();
                        groups.Add(currentGroup);
                    }
                    workingLine = workingLine.Replace("[", "").Replace("]", "");
                    string[] parts = workingLine.Split("=".ToCharArray());

                    if (parts.Length != 2)
                    {
                        currentGroup.SetString("direction", parts[0]);
                    }
                    else
                    {
                        currentGroup.SetString(parts[0], parts[1]);
                    }
                    continue;
                }
                if (workingLine.StartsWith("COUNT"))
                {
                    string[] parts = workingLine.Split("=".ToCharArray());
                    int caseId = -1;
                    int.TryParse(parts[1].Trim(), out caseId);
                    currentTestCase = new TestCase { TestCaseId = caseId };
                    currentGroup.Tests.Add(currentTestCase);
                    continue;
                }

                if (workingLine.StartsWith("DataUnitLen"))
                {
                    string[] parts = workingLine.Split("=".ToCharArray());
                    var prevDataLen = currentGroup.PayloadLen;
                    currentGroup.SetString(parts[0].Trim(), parts[1].Trim());

                    if (prevDataLen != currentGroup.PayloadLen)
                    {
                        inCases = false;
                        var prevDirection = currentGroup.Direction;
                        currentGroup = new TestGroup();
                        currentGroup.Direction = prevDirection;
                        groups.Add(currentGroup);
                    }

                    continue;
                }

                inCases = true;
                string[] valueParts = workingLine.Split("=".ToCharArray());
                currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
            }

            var testVectorSet = new TestVectorSet { TestGroups = groups.Select(g => g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}
