using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.Parsers
{
    public class LegacyResponseFileParserAes : LegacyResponseFileParserBase
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
                                  @"Key = (?<key>.*)\s*" +
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

                currentGroup.KeyLength = int.Parse(matches[0].Groups["klen"].Value);
                currentGroup.MessageLength = int.Parse(matches[0].Groups["mlen"].Value);
                currentGroup.MacLength = int.Parse(matches[0].Groups["tlen"].Value);
                groups.Add(currentGroup);
                foreach (Match match in matches)
                {
                    var klen = int.Parse(match.Groups["klen"].Value);
                    var mlen = int.Parse(match.Groups["mlen"].Value) * 8;
                    var tlen = int.Parse(match.Groups["tlen"].Value) * 8;
                    var key = new BitString(match.Groups["key"].Value);
                    var msg = new BitString(match.Groups["msg"].Value != "00" ? match.Groups["msg"].Value : "");
                    var mac = new BitString(match.Groups["mac"].Value);
                    var result = match.Groups["result"].Value;

                    if (klen != currentGroup.KeyLength || mlen != currentGroup.MessageLength ||
                        tlen != currentGroup.MacLength)
                    {
                        currentGroup = CreateTestGroup(file);
                        currentGroup.KeyLength = klen;
                        currentGroup.MessageLength = mlen;
                        currentGroup.MacLength = tlen;
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

        protected override TestGroup CreateTestGroup(string path)
        {
            var testGroup = new TestGroup();

            string fileName = Path.GetFileName(path).ToLower();

            //make sure we are paring AES
            if (!fileName.Contains("aes"))
            {
                throw new Exception("This parser only supports AES.");
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

            //parse key size
            if (fileName.Contains("128"))
            {
                testGroup.KeyLength = 128;
                testGroup.CmacType = CmacTypes.AES128;
            }
            else if (fileName.Contains("192"))
            {
                testGroup.KeyLength = 192;
                testGroup.CmacType = CmacTypes.AES192;

            }
            else if (fileName.Contains("256"))
            {
                testGroup.KeyLength = 256;
                testGroup.CmacType = CmacTypes.AES256;
            }
            else
            {
                throw new Exception("Could not determine keysize from filename.");
            }
            testGroup.Tests = new List<TestCase>();
            return testGroup;
        }
    }
}
