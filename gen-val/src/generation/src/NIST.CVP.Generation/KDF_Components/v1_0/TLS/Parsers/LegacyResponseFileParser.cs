﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.KDF_Components.v1_0.TLS.Parsers
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
                        currentGroup = new TestGroup();
                        groups.Add(currentGroup);
                    }

                    workingLine = workingLine.Replace("[", "").Replace("]", "");
                    var parts = workingLine.Split("=".ToCharArray());

                    if (parts.Length == 1)
                    {
                        var commaParts = parts[0].Split(",".ToCharArray());
                        if (commaParts.Length == 1)
                        {
                            currentGroup.SetString("hashalg", "sha-1");
                            currentGroup.SetString("tlsversion", "v1.0/1.1");
                            continue;
                        }

                        currentGroup.SetString("tlsversion", "v1.2");
                        currentGroup.SetString("hashalg", commaParts[1].Trim());
                        continue;
                    }

                    currentGroup.SetString(parts[0].Trim(), parts[1].Trim());
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
                currentTestCase.SetString(valueParts[0].Trim(), valueParts[1].Trim());
            }

            var testVectorSet = new TestVectorSet { Algorithm = "kdf-components", Mode = "tls", TestGroups = groups.Select(g => g).ToList() };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }
    }
}