using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
    {
        public ParseResponse<TestVectorSet> Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new ParseResponse<TestVectorSet>("There was no path supplied");
            }

            if (!File.Exists(path))
            {
                return new ParseResponse<TestVectorSet>($"Could not file file: {path}");
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

            var groups = new List<TestGroup>();
            TestGroup curGroup = null;
            TestCase curTestCase = null;
            var inCases = false;
            var caseId = 1;

            foreach (var line in lines)
            {
                var workingLine = line.ToLower().Trim();
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
                    if (curGroup == null || inCases)
                    {
                        inCases = false;
                        curGroup = new TestGroup();
                        groups.Add(curGroup);
                    }

                    workingLine = workingLine.Replace("[", "").Replace("]", "");
                    var propParts = workingLine.Split(",");
                    foreach (var prop in propParts)
                    {
                        if (prop.Trim().Contains("sha"))
                        {
                            curGroup.SetString("hashAlg", prop.Trim());
                        }
                        else
                        {
                            curGroup.SetString("curve", prop.Trim());
                        }
                    }

                    continue;
                }

                if (workingLine.StartsWith("msg = "))
                {
                    curTestCase = new TestCase { TestCaseId = caseId };
                    inCases = true;

                    curGroup.Tests.Add(curTestCase);
                    caseId++;
                }

                var parts = workingLine.Split("=");
                curTestCase.SetString(parts[0].Trim(), parts[1].Trim());
            }

            return new ParseResponse<TestVectorSet>(new TestVectorSet { Algorithm = "ECDSA", Mode = "SigGen", TestGroups = groups.Select(g => g).ToList() });
        }
    }
}
