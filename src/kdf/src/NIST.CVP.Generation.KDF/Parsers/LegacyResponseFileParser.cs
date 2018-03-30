using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.KDF.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet>
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

            var kdfMode = "counter";
            var fileName = Path.GetFileName(path).ToLower();
            if (fileName.Contains("kdfctr"))
            {
                kdfMode = "counter";
            }
            else if (fileName.Contains("feedback"))
            {
                kdfMode = "feedback";
            }
            else if (fileName.Contains("dblpipeline"))
            {
                kdfMode = "double pipeline iteration";
            }
            else
            {
                throw new Exception();
            }

            var groups = new List<TestGroup>();
            TestGroup currentGroup = null;
            TestCase currentTestCase = null;
            var inCases = false;
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
                        currentGroup = new TestGroup
                        {
                            KdfMode = EnumHelpers.GetEnumFromEnumDescription<KdfModes>(kdfMode)
                        };
                        groups.Add(currentGroup);
                    }

                    workingLine = workingLine.Replace("[", "").Replace("]", "");
                    var parts = workingLine.Split("=".ToCharArray());

                    currentGroup.SetString(parts[0], parts[1]);
                    continue;
                }

                if (workingLine.StartsWith("COUNT"))
                {
                    var parts = workingLine.Split("=".ToCharArray());
                    int.TryParse(parts[1].Trim(), out var caseId);
                    currentTestCase = new TestCase { TestCaseId = caseId };
                    currentGroup.Tests.Add(currentTestCase);
                    continue;
                }

                inCases = true;
                var valueParts = workingLine.Split("=".ToCharArray());

                // Empty IV case
                if (valueParts.Length == 1)
                {
                    currentTestCase.SetString(valueParts[0].Trim(), "");
                    continue;
                }

                currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
            }

            var testVectorSet = new TestVectorSet { Algorithm = "KDF", Mode = "", TestGroups = groups.Select(g => (ITestGroup)g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}
