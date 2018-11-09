using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CCM.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
    {
        public ParseResponse<TestVectorSet> Parse(string path)
        {

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

                string direction = "encrypt";
                string fileName = Path.GetFileName(file);

                // Decrypt operations are only performed on DVPT file
                if (fileName.StartsWith("DVPT", StringComparison.OrdinalIgnoreCase))
                {
                    direction = "decrypt";
                }

                bool nonceForEntireGroup = fileName.StartsWith("VADT", StringComparison.OrdinalIgnoreCase) ||
                                           fileName.StartsWith("VPT", StringComparison.OrdinalIgnoreCase) ||
                                           fileName.StartsWith("VTT", StringComparison.OrdinalIgnoreCase);
                
                TestGroup currentGroup = null;
                TestCase currentTestCase = null;
                BitString key = null;
                BitString nonce = null;
                var aLen = 0;
                var pLen = 0;
                var nLen = 0;
                var tLen = 0;
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
                    
                    // New test group when "[" encountered
                    if (workingLine.StartsWith("["))
                    {
                        
                        workingLine = workingLine.Replace("[", "").Replace("]", "");

                        // Get the lengths in working line
                        string[] lengthParts = workingLine.Split(",".ToCharArray());
                        SetLengths(lengthParts, ref aLen, ref pLen, ref nLen, ref tLen);

                        currentGroup = new TestGroup()
                        {
                            Function = direction,
                            KeyLength = 0,
                            TestType = file.Contains("MCT") ? "MCT" : string.Empty,
                            TagLength = tLen * 8,
                            AADLength = aLen * 8,
                            PayloadLength = pLen * 8,
                            IVLength = nLen * 8
                        };
                        groups.Add(currentGroup);
                        continue;
                    }

                    // Get the key for the group
                    if (workingLine.StartsWith("key", StringComparison.OrdinalIgnoreCase))
                    {
                        var keyParts = SplitLabelFromValue(workingLine);
                        key = new BitString(keyParts[1]);
                        continue;
                    }

                    // Get the nonce for the group when appropriate
                    if (workingLine.StartsWith("nonce", StringComparison.OrdinalIgnoreCase)
                        && nonceForEntireGroup)
                    {
                        var nonceParts = SplitLabelFromValue(workingLine);
                        nonce = new BitString(nonceParts[1]);
                        continue;
                    }

                    // If setting a group length, continue, as they are not a part of a test case
                    if (IsLengthLine(workingLine))
                    {
                        SetLengths(workingLine, ref aLen, ref pLen, ref nLen, ref tLen);
                        continue;
                    }
                    
                    // New test case
                    if (workingLine.StartsWith("Count", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] parts = workingLine.Split("=".ToCharArray());
                        int caseId = -1;
                        int.TryParse(parts[1].Trim(), out caseId);
                        currentTestCase = new TestCase
                        {
                            TestCaseId = caseId,
                            Key = key,
                            IV = nonce,
                            TestPassed = true
                        };

                        currentGroup.Tests.Add(currentTestCase);
                        continue;
                    }

                    // Check for failure test
                    if (workingLine.StartsWith("Result", StringComparison.OrdinalIgnoreCase))
                    {
                        var resultParts = SplitLabelFromValue(workingLine);
                        if (resultParts[1].StartsWith("Fail", StringComparison.OrdinalIgnoreCase))
                        {
                            currentTestCase.TestPassed = false;
                        }
                        continue;
                    }

                    var valueParts = SplitLabelFromValue(workingLine);
                    currentTestCase.SetString(valueParts[0], valueParts[1]);

                    if (currentGroup.AADLength == 0)
                    {
                        currentTestCase.AAD = new BitString(0);
                    }
                    if (currentGroup.PayloadLength == 0)
                    {
                        currentTestCase.PlainText = new BitString(0);
                    }
                }

            }
            var testVectorSet = new TestVectorSet { Algorithm = "AES-CCM", TestGroups = groups.Select(g => g).ToList()};
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private bool IsLengthLine(string workingLine)
        {
            return workingLine.StartsWith("alen", StringComparison.OrdinalIgnoreCase) ||
                   workingLine.StartsWith("plen", StringComparison.OrdinalIgnoreCase) ||
                   workingLine.StartsWith("nlen", StringComparison.OrdinalIgnoreCase) ||
                   workingLine.StartsWith("tlen", StringComparison.OrdinalIgnoreCase);
        }

        private void SetLengths(string[] lengthParts, ref int aLen, ref int pLen, ref int nLen, ref int tLen)
        {
            foreach (var lengthPart in lengthParts)
            {
                SetLengths(lengthPart, ref aLen, ref pLen, ref nLen, ref tLen);
            }
        }

        private void SetLengths(string lengthPart, ref int aLen, ref int pLen, ref int nLen, ref int tLen)
        {
            var valueParts = SplitLabelFromValue(lengthPart);
            switch (valueParts[0].ToLower())
            {
                case "alen":
                    SetLength(ref aLen, valueParts[1]);
                    break;
                case "plen":
                    SetLength(ref pLen, valueParts[1]);
                    break;
                case "nlen":
                    SetLength(ref nLen, valueParts[1]);
                    break;
                case "tlen":
                    SetLength(ref tLen, valueParts[1]);
                    break;
            }
        }

        private void SetLength(ref int varToSet, string valueToSet)
        {
            int val = 0;
            int.TryParse(valueToSet, out val);
            varToSet = val;
        }

        private string[] SplitLabelFromValue(string workingLine)
        {
            string[] valueParts = workingLine.Split("=".ToCharArray());
            valueParts[0] = valueParts[0].Trim();
            valueParts[1] = valueParts[1].Trim();

            return valueParts;
        }

        private static Logger ThisLogger => LogManager.GetLogger("LegacyResponseFileParser");
    }
}
