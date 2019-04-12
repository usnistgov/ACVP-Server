using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.HMAC.v1_0.Parsers
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
                int caseId = -1;
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

                    // "[" = new group
                    if (workingLine.StartsWith("["))
                    {
                        ModeValues shaMode = ModeValues.SHA1;
                        DigestSizes shaDigestSize = DigestSizes.d160;

                        workingLine = workingLine.Replace("[", "").Replace("]", "");
                        SetGroupOptions(workingLine, ref shaMode, ref shaDigestSize);

                        currentGroup = new TestGroup()
                        {
                            ShaDigestSize = shaDigestSize,
                            ShaMode = shaMode,
                            MessageLength = 128, // always 128
                            TestType = "AFT"
                        };

                        groups.Add(currentGroup);

                        continue;
                    }

                    string[] parts = workingLine.Split("=".ToCharArray());
                    parts[0] = parts[0].Trim();
                    parts[1] = parts[1].Trim();

                    // New test case on "count"
                    if (parts[0].StartsWith("count"))
                    {
                        int.TryParse(parts[1].Trim(), out caseId);

                        currentTestCase = new TestCase()
                        {
                            TestCaseId = caseId
                        };

                        currentGroup.Tests.Add(currentTestCase);
                        
                        continue;
                    }

                    if (parts[0].Contains("len"))
                    {
                        currentGroup.SetString(parts[0], parts[1]);

                        continue;
                    }

                    currentTestCase.SetString(parts[0], parts[1] == "00" ? string.Empty : parts[1].Trim());
                }

            }

            var testVectorSet = new TestVectorSet { Algorithm = $"HMAC", TestGroups = groups.Select(g => g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private void SetGroupOptions(string workingLine, ref ModeValues shaMode, ref DigestSizes shaDigestSize)
        {
            // workingLine in format "[L=20 SHAAlg=SHA_2]"
            // Where "L" is the digest size (in bytes), SHAAlg is the Sha mode.

            var pieces = workingLine.Split(" ".ToCharArray());

            var digestSizePieces = pieces[0].Split("=".ToCharArray());
            
            switch (digestSizePieces[1].Trim())
            {
                case "20":
                    shaDigestSize = DigestSizes.d160;
                    break;
                case "28":
                    shaDigestSize = DigestSizes.d224;
                    break;
                case "32":
                    shaDigestSize = DigestSizes.d256;
                    break;
                case "48":
                    shaDigestSize = DigestSizes.d384;
                    break;
                case "64":
                    shaDigestSize = DigestSizes.d512;
                    break;
                case "512224":
                    shaDigestSize = DigestSizes.d512t224;
                    break;
                case "512256":
                    shaDigestSize = DigestSizes.d512t256;
                    break;
                default:
                    throw new ArgumentException(nameof(shaDigestSize));
            }

            var shaModePieces = pieces[1].Split("=".ToCharArray());
            switch (shaModePieces[1].Trim())
            {
                case "sha_2":
                    shaMode = ModeValues.SHA2;

                    // Weird
                    if (shaDigestSize == DigestSizes.d160)
                    {
                        shaMode = ModeValues.SHA1;
                    }

                    break;
                case "sha_3":
                    shaMode = ModeValues.SHA3;
                    break;
                default:
                    throw new ArgumentException(nameof(shaMode));
            }

        }
    }
}
