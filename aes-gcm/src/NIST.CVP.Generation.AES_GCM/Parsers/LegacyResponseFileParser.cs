using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.Parsers
{
    public class LegacyResponseFileParser
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

            var groups = new List<TestGroup>();
            TestGroup currentGroup = null;
            foreach (var line in lines)
            {
                var  workingLine = line.Trim();
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
                    if (currentGroup == null)
                    {
                        currentGroup = new TestGroup();
                    }
                }



            }


            return  new ParseResponse<TestVectorSet>("Not done yet.");
        }
    }
}
