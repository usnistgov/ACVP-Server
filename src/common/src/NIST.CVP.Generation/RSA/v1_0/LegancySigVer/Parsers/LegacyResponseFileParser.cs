using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.RSA.v1_0.SigVer;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA.v1_0.LegancySigVer.Parsers
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
            var curMod = 0;
            BigInteger curN = 0;
            var saltLen = 0;
            var inCases = false;
            var caseId = 1;

            foreach(var line in lines)
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
                    if(curGroup == null || inCases)
                    {
                        inCases = false;
                        curGroup = new TestGroup
                        {
                            Modulo = curMod,
                            Key = new KeyPair {PubKey = new PublicKey {N = curN}},
                            SaltLen = saltLen
                        };
                        groups.Add(curGroup);
                    }

                    workingLine = workingLine.Replace("[", "").Replace("]", "");
                    var propParts = workingLine.Split("=".ToCharArray());

                    if (propParts[0].Trim() == "mod")
                    {
                        curMod = int.Parse(propParts[1].Trim());
                        curGroup.Modulo = curMod;
                        continue;
                    }

                    if (propParts[0].Trim() == "saltlen")
                    {
                        saltLen = int.Parse(propParts[1].Trim());
                        curGroup.SaltLen = saltLen;
                        continue;
                    }

                    if (propParts[0].Trim() == "n")
                    {
                        curN = new BitString(propParts[1].Trim()).ToPositiveBigInteger();
                        curGroup.Key.PubKey.N = curN;
                        continue;
                    }

                    curGroup.SetString(propParts[0].Trim(), propParts[1].Trim());

                    continue;
                }

                if (workingLine.StartsWith("e = "))
                {
                    var propParts = workingLine.Split("=".ToCharArray());
                    curGroup.Key.PubKey.E = new BitString(propParts[1].Trim()).ToPositiveBigInteger();
                    continue;
                }

                if (workingLine.StartsWith("d = ") || workingLine.StartsWith("p = ") || workingLine.StartsWith("q = "))
                {
                    continue;
                }

                if(workingLine.StartsWith("msg = "))
                {
                    curTestCase = new TestCase { TestCaseId = caseId };
                    inCases = true;

                    curGroup.Tests.Add(curTestCase);
                    caseId++;
                }

                var parts = workingLine.Split("=".ToCharArray());
                curTestCase.SetString(parts[0].Trim(), parts[1].Trim());
            }

            return new ParseResponse<TestVectorSet>(new TestVectorSet { Algorithm = "RSA", Mode = "LegacySigVer", TestGroups = groups });
        }
    }
}
