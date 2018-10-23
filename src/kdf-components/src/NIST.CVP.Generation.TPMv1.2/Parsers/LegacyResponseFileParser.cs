using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NIST.CVP.Generation.TPMv1._2.Parsers
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

            var group = new TestGroup();
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

                if (workingLine.StartsWith("COUNT"))
                {
                    var parts = workingLine.Split("=".ToCharArray());
                    int.TryParse(parts[1].Trim(), out var caseId);
                    currentTestCase = new TestCase { TestCaseId = caseId };
                    group.Tests.Add(currentTestCase);
                    continue;
                }

                var valueParts = workingLine.Split("=".ToCharArray());
                currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
            }

            var testVectorSet = new TestVectorSet
            {
                Algorithm = "kdf-components",
                Mode = "tpm",
                TestGroups = new List<TestGroup>
                {
                    group
                }
            };

            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}
