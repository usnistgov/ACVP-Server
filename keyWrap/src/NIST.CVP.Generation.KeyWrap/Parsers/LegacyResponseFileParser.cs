using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap.Parsers
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

                KeyWrapType keyWrapType = 0;
                string direction = string.Empty;
                string kwCipher = string.Empty;
                int keySize = 0;

                GetFileInformation(file, ref keyWrapType, ref direction, ref kwCipher, ref keySize);

                TestGroup currentGroup = null;
                TestCase currentTestCase = null;

                foreach (var line in lines)
                {
                    var workingLine = line.Trim().ToLower();
                    if (string.IsNullOrEmpty(workingLine))
                    {
                        continue;
                    }
                    if (workingLine.StartsWith("#"))
                    {
                        continue;
                    }

                    // New group when "[" encountered
                    if (workingLine.StartsWith("["))
                    {
                        currentGroup = new TestGroup()
                        {
                            Direction = direction,
                            KeyLength = keySize,
                            KeyWrapType = keyWrapType,
                            KwCipher = kwCipher,
                            Tests = new List<ITestCase>()
                        };
                        groups.Add(currentGroup);

                        workingLine = workingLine.Replace("[", "").Replace("]", "");

                        string[] groupParts = workingLine.Split("=".ToCharArray());
                        currentGroup.SetString(groupParts[0].Trim(), groupParts[1].Trim());

                        continue;
                    }

                    string[] parts = workingLine.Split("=".ToCharArray());

                    // begins with count means start of a test case
                    if (workingLine.StartsWith("count", StringComparison.OrdinalIgnoreCase))
                    {
                        int caseId = -1;
                        int.TryParse(parts[1].Trim(), out caseId);
                        currentTestCase = new TestCase {TestCaseId = caseId};
                        currentGroup.Tests.Add(currentTestCase);
                        continue;
                    }

                    if (workingLine.StartsWith("fail"))
                    {
                        currentTestCase.FailureTest = true;
                        continue;
                    }

                    currentTestCase.SetString(parts[0].Trim(), parts[1].Trim());
                }
            }

            var testVectorSet = new TestVectorSet
            {
                Algorithm = "KeyWrap",
                TestGroups = groups.Select(g => (ITestGroup) g).ToList()
            };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private void GetFileInformation(string file, ref KeyWrapType keyWrapType, ref string direction, ref string kwCipher, ref int keySize)
        {
            FileInfo fileInfo = new FileInfo(file);

            string fileName = fileInfo.Name.ToLower();

            if (fileName.Contains("kw_"))
            {
                keyWrapType = KeyWrapType.AES_KW;
            }

            direction = fileName.Contains("_ae_") ? "encrypt" : "decrypt";
            kwCipher = fileName.Contains("_inv") ? "inverse" : "cipher";

            if (fileName.Contains("128"))
            {
                keySize = 128;
            }
            else if (fileName.Contains("192"))
            {
                keySize = 192;
            }
            else if (fileName.Contains("256"))
            {
                keySize = 256;
            }
        }

    }
}
