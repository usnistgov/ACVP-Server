using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0.Parsers
{
    public class LegacyResponseFileParserTdes : LegacyResponseFileParserBase
    {
        public override ParseResponse<TestVectorSet> Parse(string path)
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
            var regex = new Regex(@"Count = (?<count>.*)\s*" +
                                  @"Klen = (?<klen>.*)\s*" +
                                  @"Mlen = (?<mlen>.*)\s*" +
                                  @"Tlen = (?<tlen>.*)\s*" +
                                  @"Key1 = (?<key1>.*)\s*" +
                                  @"Key2 = (?<key2>.*)\s*" +
                                  @"Key3 = (?<key3>.*)\s*" +
                                  @"Msg = (?<msg>.*)\s*" +
                                  @"Mac = (?<mac>.*)\s*" +
                                  @"(?:Result = (?<result>.*)\s*)?");

            var files = Directory.GetFiles(path, "*.fax");
            foreach (var file in files)
            {
                string fileContent;
                try
                {
                    fileContent = File.ReadAllText(file).Replace("\r\n", "\n");
                    ;
                }
                catch (Exception ex)
                {
                    return new ParseResponse<TestVectorSet>(ex.Message);
                }
                var matches = regex.Matches(fileContent);
                var currentGroup = CreateTestGroup(file);

                var matchedGroups = matches[0].Groups;
                currentGroup.KeyingOption = KLenToKeyingOption(int.Parse(matchedGroups["klen"].Value));
                currentGroup.MessageLength = int.Parse(matchedGroups["mlen"].Value);
                currentGroup.MacLength = int.Parse(matchedGroups["tlen"].Value);
                groups.Add(currentGroup);
                foreach (Match match in matches)
                {
                    var matchGroups = match.Groups;

                    var keyingOption = KLenToKeyingOption(int.Parse(matchGroups["klen"].Value));
                    var messageLength = int.Parse(matchGroups["mlen"].Value) * 8;
                    var macLenghth = int.Parse(matchGroups["tlen"].Value) * 8;
                    var key = new BitString(matchGroups["key1"].Value)
                            .ConcatenateBits(new BitString(matchGroups["key2"].Value))
                            .ConcatenateBits(new BitString(matchGroups["key3"].Value));
                    var msg = new BitString(matchGroups["msg"].Value != "00" ? matchGroups["msg"].Value : "");
                    var mac = new BitString(matchGroups["mac"].Value);
                    var result = matchGroups["result"].Value;

                    if (keyingOption != currentGroup.KeyingOption || messageLength != currentGroup.MessageLength ||
                        macLenghth != currentGroup.MacLength)
                    {
                        currentGroup = CreateTestGroup(file);
                        currentGroup.KeyingOption = keyingOption;
                        currentGroup.MessageLength = messageLength;
                        currentGroup.MacLength = macLenghth;
                        groups.Add(currentGroup);
                    }

                    currentGroup.Tests.Add(new TestCase()
                    {
                        Key = key,
                        Mac = mac,
                        Message = msg,
                        TestPassed = result.ToLower() == "p"
                    });
                }
            }
            var testVectorSet = new TestVectorSet
            {
                TestGroups = groups.Select(g => g).ToList()
            };
            return new ParseResponse<TestVectorSet>(testVectorSet);

        }

        private int KLenToKeyingOption(int klen)
        {
            if (klen == 3)
            {
                return 1;
            }
            else if (klen == 2)
            {
                return 2;
            }
            throw new ArgumentException($"Cannon parse klen {klen} to KeyingOption");
        }

        protected override TestGroup CreateTestGroup(string path)
        {
            var testGroup = new TestGroup();

            string fileName = Path.GetFileName(path).ToLower();

            //make sure we are paring TDES
            if (!fileName.Contains("tdes"))
            {
                throw new Exception("This parser only supports TDES.");
            }

            //parse direction
            if (fileName.Contains("gen"))
            {
                testGroup.Function = "gen";
            }
            else if (fileName.Contains("ver"))
            {
                testGroup.Function = "ver";
            }
            else
            {
                throw new Exception("Could not determine direction from filename");
            }

            testGroup.CmacType = CmacTypes.TDES;
            testGroup.Tests = new List<TestCase>();
            return testGroup;
        }
    }
}
