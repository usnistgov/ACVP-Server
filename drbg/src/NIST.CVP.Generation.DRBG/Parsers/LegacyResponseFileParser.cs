using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG.Parsers
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet>
    {
        private static DrbgMechanism _mechanism;
        private static string _mode;

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

                DrbgParameters currentParameters = null;
                TestGroup currentGroup = null;
                TestCase currentTestCase = null;

                bool nextLeftBracketIsStartOfGroup = true;
                foreach (var line in lines)
                {

                    var workingLine = line.Trim().ToLower();
                    if (string.IsNullOrEmpty(workingLine))
                    {
                        continue;
                    }
                    if (workingLine.StartsWith("#"))
                    {
                        if (workingLine.Contains("hmac"))
                        {
                            _mechanism = DrbgMechanism.HMAC;
                        }

                        if (workingLine.Contains("hash"))
                        {
                            _mechanism = DrbgMechanism.Hash;
                        }

                        continue;
                    }
                    // New test group when "[" encountered and nextLeftBracketIsStartOfGroup is true
                    if (workingLine.StartsWith("[") && nextLeftBracketIsStartOfGroup)
                    {
                        currentParameters = new DrbgParameters();
                        currentGroup = new TestGroup();
                        workingLine = workingLine.Replace("[", "").Replace("]", "");
                        _mode = workingLine.Trim();

                        SetMechanismAndMode(currentParameters, workingLine);
                        SetDerivationFunction(currentParameters, workingLine);

                        currentGroup.DrbgParameters = currentParameters;

                        groups.Add(currentGroup);
                        nextLeftBracketIsStartOfGroup = false;

                        continue;
                    }
                    if (workingLine.StartsWith("["))
                    {
                        workingLine = workingLine.Replace("[", "").Replace("]", "");

                        string[] groupParts = workingLine.Split("=".ToCharArray());
                        currentGroup.SetString(groupParts[0].Trim(), groupParts[1].Trim());

                        continue;
                    }

                    string[] parts = workingLine.Split("=".ToCharArray());

                    // begins with count means start of a test case
                    // Additionally, the next time a [ is encountered, signals a new test group
                    if (workingLine.StartsWith("count", StringComparison.OrdinalIgnoreCase))
                    {
                        nextLeftBracketIsStartOfGroup = true;

                        int caseId = -1;
                        int.TryParse(parts[1].Trim(), out caseId);
                        currentTestCase = new TestCase {TestCaseId = caseId};
                        currentGroup.Tests.Add(currentTestCase);
                        continue;
                    }

                    if (workingLine.Contains("reseed"))
                    {
                        currentParameters.ReseedImplemented = true;
                        currentGroup.ReSeed = true;
                    }

                    // EntropyInputReseed is the start of a new "OtherInput"
                    if (workingLine.StartsWith("EntropyInputReseed", StringComparison.OrdinalIgnoreCase))
                    {
                        currentTestCase.OtherInput.Add(new OtherInput());
                        int lastIndex = currentTestCase.OtherInput.Count - 1;
                        currentTestCase.OtherInput[lastIndex].EntropyInput = new BitString(parts[1].Trim());
                        continue;
                    }
                    // The second part of an "OtherInput"
                    if (workingLine.StartsWith("AdditionalInputReseed", StringComparison.OrdinalIgnoreCase))
                    {
                        int lastIndex = currentTestCase.OtherInput.Count - 1;
                        currentTestCase.OtherInput[lastIndex].AdditionalInput = new BitString(parts[1].Trim());
                        continue;
                    }

                    // The Entropy and Additional input values are in the reverse order as above (for some reason)
                    // For a PR true scenario, additional input if the start of a new "OtherInput"
                    if (workingLine.StartsWith("AdditionalInput", StringComparison.OrdinalIgnoreCase) &&
                        !workingLine.StartsWith("AdditionalInputReseed", StringComparison.OrdinalIgnoreCase))
                    {
                        currentTestCase.OtherInput.Add(new OtherInput());
                        int lastIndex = currentTestCase.OtherInput.Count - 1;
                        currentTestCase.OtherInput[lastIndex].AdditionalInput = new BitString(parts[1].Trim());
                        continue;
                    }

                    if (workingLine.StartsWith("EntropyInputPR", StringComparison.OrdinalIgnoreCase))
                    {
                        int lastIndex = currentTestCase.OtherInput.Count - 1;
                        currentTestCase.OtherInput[lastIndex].EntropyInput = new BitString(parts[1].Trim());
                        continue;
                    }

                    currentTestCase.SetString(parts[0].Trim(), parts[1].Trim());
                }
            }

            var testVectorSet = new TestVectorSet
            {
                Algorithm = "ctrDRBG",
                TestGroups = groups.Select(g => (ITestGroup) g).ToList()
            };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

        private static void SetDerivationFunction(DrbgParameters currentParameters, string workingLine)
        {
            if (workingLine.Contains("use df"))
            {
                currentParameters.DerFuncEnabled = true;
            }
            else
            {
                currentParameters.DerFuncEnabled = false;
            }
        }

        private static void SetMechanismAndMode(DrbgParameters currentParameters, string workingLine)
        {
            if (workingLine.Contains("aes"))
            {
                currentParameters.Mechanism = DrbgMechanism.Counter;

                if (workingLine.Contains("128"))
                {
                    currentParameters.Mode = DrbgMode.AES128;
                    currentParameters.SecurityStrength = 128;
                }
                else if (workingLine.Contains("192"))
                {
                    currentParameters.Mode = DrbgMode.AES192;
                    currentParameters.SecurityStrength = 192;
                }
                else if (workingLine.Contains("256"))
                {
                    currentParameters.Mode = DrbgMode.AES256;
                    currentParameters.SecurityStrength = 256;
                }
                else
                {
                    throw new FileLoadException("Invalid mode encountered");
                }
            }
            else if (workingLine.Contains("tdea"))
            {
                currentParameters.Mechanism = DrbgMechanism.Counter;
                currentParameters.Mode = DrbgMode.TDES;
                currentParameters.SecurityStrength = 112;
            }
            else if (_mechanism == DrbgMechanism.HMAC || _mechanism == DrbgMechanism.Hash)
            {
                currentParameters.Mechanism = _mechanism;
                if (_mode.Contains("sha-1"))
                {
                    currentParameters.Mode = DrbgMode.SHA1;
                    currentParameters.SecurityStrength = 160;
                }
                else if (_mode.Contains("sha-224"))
                {
                    currentParameters.Mode = DrbgMode.SHA224;
                    currentParameters.SecurityStrength = 224;
                }
                else if (_mode.Contains("sha-256"))
                {
                    currentParameters.Mode = DrbgMode.SHA256;
                    currentParameters.SecurityStrength = 256;
                }
                else if (_mode.Contains("sha-384"))
                {
                    currentParameters.Mode = DrbgMode.SHA384;
                    currentParameters.SecurityStrength = 384;
                }
                // Do these checks first because they both contain "sha-512"...
                else if (_mode.Contains("sha-512/224"))
                {
                    currentParameters.Mode = DrbgMode.SHA512t224;
                    currentParameters.SecurityStrength = 224;
                }
                else if (_mode.Contains("sha-512/256"))
                {
                    currentParameters.Mode = DrbgMode.SHA512t256;
                    currentParameters.SecurityStrength = 256;
                }
                else if (_mode.Contains("sha-512"))
                {
                    currentParameters.Mode = DrbgMode.SHA512;
                    currentParameters.SecurityStrength = 512;
                }
                else
                {
                    throw new FileLoadException("Invalid mode encountered");
                }
            }
            else
            {
                throw new FileLoadException("Invalid Mechanism encountered");
            }
        }
    }
}
