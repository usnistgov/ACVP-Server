using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.CMAC.Parsers
{
    public class LegacyResponseFileParser<TTestVectorSet, TTestGroup, TTestCase> : ILegacyResponseFileParser<TTestVectorSet>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>, new()
        where TTestGroup : TestGroupBase<TTestCase>, new()
        where TTestCase : TestCaseBase, new()
    {
        public ParseResponse<TTestVectorSet> Parse(string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                return new ParseResponse<TTestVectorSet>("There was no path supplied.");
            }

            if (!Directory.Exists(path))
            {
                return new ParseResponse<TTestVectorSet>($"Could not find path {path}");
            }

            var groups = new List<TTestGroup>();

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
                    return new ParseResponse<TTestVectorSet>(ex.Message);
                }
                string direction = string.Empty;
                int keySize = 0;
                CmacTypes cmacType = CmacTypes.AES128;
                SetFileGroupOptions(file, ref direction, ref keySize, ref cmacType);

                TTestGroup currentGroup = null;
                TTestCase currentTestCase = null;
                bool startNewGroup = false;
                int currentMsgLen = 0;
                int currentMacLen = 0;
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

                    string[] parts = workingLine.Split("=".ToCharArray());

                    if (parts[0].StartsWith("count"))
                    {
                        int.TryParse(parts[1].Trim(), out caseId);
                        continue;
                    }

                    if (parts[0].Contains("len"))
                    {
                        var label = parts[0].Trim();
                        var value = int.Parse(parts[1]);

                        if (label == "mlen")
                        {
                            if (currentGroup != null && currentMsgLen != value)
                            {
                                startNewGroup = true;
                            }
                            currentMsgLen = value;
                        }
                        if (label == "tlen")
                        {
                            if (currentGroup != null && currentMacLen != value)
                            {
                                startNewGroup = true;
                            }
                            currentMacLen = value;
                        }
                        continue;
                    }

                    if (parts[0].StartsWith("key"))
                    {
                        if (currentGroup == null || startNewGroup)
                        {
                            startNewGroup = false;
                            
                            currentGroup = new TTestGroup
                            {
                                Function = direction,
                                CmacType = cmacType,
                                KeyLength = keySize,
                                MessageLength = currentMsgLen * 8,
                                MacLength = currentMacLen * 8,
                                Tests = new List<ITestCase>()
                            };
                            groups.Add(currentGroup);
                        }

                        currentTestCase = new TTestCase {TestCaseId = caseId};
                        currentGroup.Tests.Add(currentTestCase);
                    }

                    if (parts[0].StartsWith("result"))
                    {
                        if (parts[1].Trim().StartsWith("p"))
                        {
                            currentTestCase.FailureTest = false;
                        }
                        else
                        {
                            currentTestCase.FailureTest = true;
                        }
                        continue;
                    }

                    currentTestCase.SetString(parts[0].Trim(), parts[1].Trim() == "00" ? string.Empty : parts[1].Trim());
                }

            }

            var testVectorSet = new TTestVectorSet { Algorithm = $"CMAC-AES", TestGroups = groups.Select(g => (ITestGroup)g).ToList() };
            return new ParseResponse<TTestVectorSet>(testVectorSet);
        }

        private void SetFileGroupOptions(string path, ref string direction, ref int keySize, ref CmacTypes cmacType)
        {
            string fileName = Path.GetFileName(path).ToLower();
            if (fileName.Contains("gen"))
            {
                direction = "gen";
            }
            else if (fileName.Contains("ver"))
            {
                direction = "ver";
            }
            else
            {
                throw new Exception("Could not determine direction from filename");
            }

            if (fileName.Contains("128"))
            {
                keySize = 128;
                cmacType = CmacTypes.AES128;
            }
            else if (fileName.Contains("192"))
            {
                keySize = 192;
                cmacType = CmacTypes.AES192;
            }
            else if (fileName.Contains("256"))
            {
                keySize = 256;
                cmacType = CmacTypes.AES256;
            }
            else
            {
                throw new Exception("Could not determine keysize from filename.");
            }
        }
    }
}
