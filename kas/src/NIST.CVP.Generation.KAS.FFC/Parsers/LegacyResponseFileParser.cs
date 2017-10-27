using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.Core.Parsers;
using NLog.LayoutRenderers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC.Parsers
{
    /// <summary>
    /// Note that KAS files are exceedingly complex, 
    /// limiting usage of a single MAC/parameter set will save time on parsing
    /// </summary>
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet>
    {
        private const string Algorithm = "KAS-FFC";
        private readonly BitString _serverId = new BitString("434156536964");
        private readonly BitString _iutId = new BitString("a1b2c3d4e5");

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
                string fileName = Path.GetFileName(file).ToLower();
                var lines = new List<string>();
                try
                {
                    lines = File.ReadAllLines(file).ToList();
                }
                catch (Exception ex)
                {
                    return new ParseResponse<TestVectorSet>(ex.Message);
                }

                FfcScheme scheme = FfcScheme.DhEphem;
                KasMode kasMode = KasMode.NoKdfNoKc;
                KeyAgreementRole iutKasRole = KeyAgreementRole.InitiatorPartyU;
                KeyConfirmationRole iutKeyConfRole = KeyConfirmationRole.None;
                KeyConfirmationDirection iutKeyConfDirection = KeyConfirmationDirection.None;
                string kdfType = string.Empty;

                GetKasOptionsFromFileName(fileName, ref scheme, ref kasMode, ref iutKasRole, ref iutKeyConfRole, ref iutKeyConfDirection, ref kdfType);

                // limited ot a single group per file... see note at class start
                TestGroup currentGroup = new TestGroup()
                {
                    Tests = new List<ITestCase>(),
                    OiPattern = "uPartyInfo||vPartyInfo",
                    Function = KasAssurance.DpGen|KasAssurance.DpVal|KasAssurance.FullVal|KasAssurance.KeyPairGen|KasAssurance.KeyRegen,
                    IdIut = _iutId,
                    IdIutLen = _iutId.BitLength,
                    IdServer = _serverId,
                    IdServerLen = _serverId.BitLength,
                    KasMode = kasMode,
                    Scheme = scheme,
                    KasRole = iutKasRole,
                    KcRole = iutKeyConfRole,
                    KcType = iutKeyConfDirection,
                    KdfType = kdfType,
                    TestType = "val"
                };
                groups.Add(currentGroup);
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

                    #region file header
                    #region parameter set
                    if (workingLine.StartsWith("[fb]") || workingLine.StartsWith("[fc]"))
                    {
                        currentGroup.ParmSet = GetParameterSet(workingLine.Replace("[", "").Replace("]", ""));
                        continue;
                    }
                    #endregion parameter set
                    #region kdfHash
                    string kdfHashLine = "[SHA(s) supported";
                    if (workingLine.StartsWith(kdfHashLine, StringComparison.OrdinalIgnoreCase))
                    {
                        var shaSplit = workingLine.Replace("]", "").Split(":".ToCharArray());
                        currentGroup.HashAlg = GetHashFunction(shaSplit[1]);
                        continue;
                    }
                    #endregion kdfHash
                    #region mac type
                    var macAlgoLine = "[MAC algorithm supported:  ";
                    if (workingLine.StartsWith(macAlgoLine, StringComparison.OrdinalIgnoreCase))
                    {
                        workingLine = workingLine.Replace(macAlgoLine, "", StringComparison.OrdinalIgnoreCase).Replace("]", "");
                        if (workingLine == "cmac")
                        {
                            currentGroup.MacType = KeyAgreementMacType.CmacAes;
                            continue;
                        }
                        if (workingLine == "ccm")
                        {
                            currentGroup.MacType = KeyAgreementMacType.AesCcm;
                            continue;
                        }
                        if (workingLine == "hmac")
                        {
                            // hmac relies on a sha type, which is grabbed in a separate line.
                            continue;
                        }
                    }
                    string hmacMacAlgoLine = "[HMAC SHAs supported: ";
                    if (workingLine.StartsWith(hmacMacAlgoLine, StringComparison.OrdinalIgnoreCase))
                    {
                        workingLine = workingLine.Replace(hmacMacAlgoLine, "", StringComparison.OrdinalIgnoreCase).Replace("]", "");
                        var hashFunction = GetHashFunction(workingLine);
                        switch (hashFunction.DigestSize)
                        {
                            case DigestSizes.d224:
                                currentGroup.MacType = KeyAgreementMacType.HmacSha2D224;
                                continue;
                            case DigestSizes.d256:
                                currentGroup.MacType = KeyAgreementMacType.HmacSha2D256;
                                continue;
                            case DigestSizes.d384:
                                currentGroup.MacType = KeyAgreementMacType.HmacSha2D384;
                                continue;
                            case DigestSizes.d512:
                                currentGroup.MacType = KeyAgreementMacType.HmacSha2D512;
                                continue;
                            default:
                                throw new ArgumentException($"cannot map {workingLine} to {nameof(currentGroup.MacType)}");
                        }
                    }
                    #endregion mac type
                    #region key size
                    if (workingLine.Contains("key") && workingLine.Contains("size"))
                    {
                        var keyLine = workingLine
                            .Replace("aes", "") // cmac lists key sizes as "aes128" (as example)
                            .Replace("[", "")
                            .Replace("]", "")
                            .Trim().Split(":".ToCharArray());
                        var keyValue = keyLine[1].Trim();
                        int.TryParse(keyValue, out var value);
                        currentGroup.KeyLen = value;
                        continue;
                    }
                    #endregion keysize
                    #region nonce
                    string nonceSizeLine = "[CCM NonceLen(in bytes): ";
                    if (workingLine.StartsWith(nonceSizeLine, StringComparison.OrdinalIgnoreCase))
                    {
                        workingLine = workingLine.Replace(nonceSizeLine, "", StringComparison.OrdinalIgnoreCase).Replace("]", "");
                        int.TryParse(workingLine, out var value);
                        currentGroup.AesCcmNonceLen = value * 8; // ccm is in bytes
                        continue;
                    }
                    #endregion nonce
                    #region tag
                    string ccmTagSizeLine = "[CCM Tag length(in bytes): ";
                    if (workingLine.StartsWith(ccmTagSizeLine, StringComparison.OrdinalIgnoreCase))
                    {
                        workingLine = workingLine.Replace(ccmTagSizeLine, "", StringComparison.OrdinalIgnoreCase).Replace("]", "");
                        int.TryParse(workingLine, out var value);
                        currentGroup.MacLen = value * 8; // ccm is in bytes
                        continue;
                    }
                    string cmacTagSizeLine = "[CMAC AES Tag length(in bits): ";
                    if (workingLine.StartsWith(cmacTagSizeLine, StringComparison.OrdinalIgnoreCase))
                    {
                        workingLine = workingLine.Replace(cmacTagSizeLine, "", StringComparison.OrdinalIgnoreCase).Replace("]", "");
                        int.TryParse(workingLine, out var value);
                        currentGroup.MacLen = value;
                        continue;
                    }
                    string hmacTagSizeLine = "[HMAC Tag length(in bits): ";
                    if (workingLine.StartsWith(hmacTagSizeLine, StringComparison.OrdinalIgnoreCase))
                    {
                        workingLine = workingLine.Replace(hmacTagSizeLine, "", StringComparison.OrdinalIgnoreCase).Replace("]", "");
                        int.TryParse(workingLine, out var value);
                        currentGroup.MacLen = value;
                        continue;
                    }
                    #endregion tag
                    #endregion file header

                    if (workingLine.StartsWith("["))
                    {
                        continue;
                    }

                    var splitLine = workingLine.Split("=".ToCharArray());
                    var splitName = splitLine[0].Trim();
                    var splitValue = splitLine[1].Trim();

                    #region pqg
                    if (currentGroup.SetString(splitName, splitValue))
                    {
                        continue;
                    }
                    #endregion pqg

                    #region testCase

                    // new test case on count
                    if (splitName == "count")
                    {
                        currentTestCase = new TestCase();
                        currentGroup.Tests.Add(currentTestCase);
                        currentTestCase.SetString(splitName, splitValue);
                        continue;
                    }

                    currentTestCase.SetString(splitName, splitValue);
                    #endregion testCase
                }
            }

            var testVectorSet = new TestVectorSet
            {
                Algorithm = Algorithm,
                TestGroups = groups.Select(g => (ITestGroup)g).ToList()
            };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }



        //private readonly Regex _parameterSetRegex = new Regex(
        //    // Get the parameter set label
        //    @"\[(?<parameterSet>F[B|C])\]" + 
        //    // diregard data until encountering "MAC algo..."
        //    @"(?:(?s).*?(?=\[MAC algorithm supported:  ))" +
        //    // Get the macType
        //    @"(?:\[MAC algorithm supported:  )(?<macType>.*(?=\]))" +
        //    // Everything else to a string (will parse further in separate regex)
        //    @"(?<remaining>(?s).*(?=#))"
        //);
        //private readonly Regex _shaRegex = new Regex(
        //    @"(?:(?s).*?(?=SHAs supported: ))(?:SHAs supported: )(?<sha>.*(?=\]))"
        //);
        //private readonly Regex _keySizeRegex = new Regex(
        //    @"(?:(?s).*?(?=Key Sizes supported:  ))(?:Key Sizes supported:  )(?<keySize>.*(?=\]))"
        //);
        //private readonly Regex _nonceLengthRegex = new Regex(
        //    @"(?:(?s).*?(?=NonceLen: ))(?:NonceLen: )(?<nonceLength>.*(?=\]))"
        //);
        //private readonly Regex _tagLengthRegex = new Regex(
        //    @"(?:(?s).*?(?=Tag length: ))(?:Tag length: )(?<tagLength>.*(?=\]))"
        //);
        //private readonly Regex _pqgRegex = new Regex(
        //    // PQG seperator
        //    @"\[(?<parameterSet>F[B|C]) - (?<hashFunction>SHA224|SHA256|SHA384|SHA512)\]\s*" +
        //    // Get the P
        //    @"P = (?<p>.+)\s*" +
        //    // Get the Q
        //    @"Q = (?<q>.+)\s*" +
        //    // Get the G
        //    @"G = (?<g>.+)\s*" +
        //    // Everything else (until hitting the next PQG separator, or #eof# is a part of group/testcase info
        //    @"(?<remaining>(?s).+?(?=(\[F[B|C])|#eof#))"
        //);
        //private readonly Regex _testCaseRegex = new Regex(
        //    @"COUNT = (?<COUNT>.+?)?" +
        //    @"(?:\s*?XstatCAVS = (?<XStatCAVS>.*))?" +
        //    @"(?:\s*?YstatCAVS = (?<YStatCAVS>.*))?" +
        //    @"(?:\s*?XephemCAVS = (?<XephemCAVS>.*))?" +
        //    @"(?:\s*?YephemCAVS = (?<YephemCAVS>.*))?" +
        //    @"(?:\s*?Nonce = (?<Nonce>.*))?" +
        //    @"(?:\s*?XstatIUT = (?<XstatIUT>.*))?" +
        //    @"(?:\s*?YstatIUT = (?<YstatIUT>.*))?" +
        //    @"(?:\s*?XephemIUT = (?<XephemIUT>.*))?" +
        //    @"(?:\s*?YephemIUT = (?<YephemIUT>.*))?" +
        //    @"(?:\s*?CCMNonce = (?<CCMNonce>.*))?" +
        //    @"(?:\s*?OI = (?<OI>.*))?" +
        //    @"(?:\s*?CAVSTag = (?<CAVSTag>.*))?" +
        //    @"(?:\s*?Z = (?<Z>.*))?" +
        //    @"(?:\s*?MacData = (?<MacData>.*))?" +
        //    @"(?:\s*?DKM = (?<DKM>.*))?" +
        //    @"(?:\s*?CAVSHashZZ = (?<CAVSHashZZ>.*))?" +
        //    @"(?:\s*?Result = (?<result>.*))?"
        //);

        //public ParseResponse<TestVectorSet> Parse(string path)
        //{
        //    if (string.IsNullOrEmpty(path))
        //    {
        //        return new ParseResponse<TestVectorSet>("There was no path supplied.");
        //    }

        //    if (!Directory.Exists(path))
        //    {
        //        return new ParseResponse<TestVectorSet>($"Could not find path {path}");
        //    }

        //    var groups = new List<TestGroup>();

        //    var files = Directory.GetFiles(path, "*ffc*.fax");
        //    foreach (var file in files)
        //    {
        //        // File level information (usually applied to each group)
        //        string fileName = Path.GetFileName(file).ToLower();

        //        // Test type
        //        string testType = "val";
        //        if (!fileName.Contains("kasvalidity"))
        //        {
        //            // Can't really test against CAVS impl for "AFT" tests, since the test
        //            // requires information from the IUT
        //            continue;
        //        }

        //        FfcScheme scheme = FfcScheme.DhEphem;
        //        KasMode kasMode = KasMode.NoKdfNoKc;
        //        KeyAgreementRole iutKasRole = KeyAgreementRole.InitiatorPartyU;
        //        KeyConfirmationRole iutKeyConfRole = KeyConfirmationRole.None;
        //        KeyConfirmationDirection iutKeyConfDirection = KeyConfirmationDirection.None;
        //        string kdfType = string.Empty;

        //        GetKasOptionsFromFileName(fileName, ref scheme, ref kasMode, ref iutKasRole, ref iutKeyConfRole, ref iutKeyConfDirection, ref kdfType);

        //        string fileContent;
        //        try
        //        {
        //            StringBuilder fileContentBuilder = new StringBuilder();
        //            fileContentBuilder.Append(File.ReadAllText(file));
        //            fileContentBuilder.Append($"{Environment.NewLine}#eof#");
        //            // The following replaces are done to make 
        //            // the differences in how MAC parameters are represented, consistent
        //            // Within the CAVS generated files
        //            fileContentBuilder.Replace("AES128", "128");
        //            fileContentBuilder.Replace("AES192", "192");
        //            fileContentBuilder.Replace("AES256", "256");
        //            fileContentBuilder.Replace("(in bits)", "");
        //            fileContentBuilder.Replace("(in bytes)", "");
        //            fileContentBuilder.Replace("Key sizes", "Key Sizes");
        //            fileContentBuilder.Replace("HMACKeySize", "Key Sizes supported");
        //            fileContentBuilder.Replace("\r\n", "\n");

        //            fileContent = fileContentBuilder.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            return new ParseResponse<TestVectorSet>(ex.Message);
        //        }

        //        var pqgMatches = _pqgRegex.Matches(fileContent);

        //        foreach (Match pqgMatch in pqgMatches)
        //        {
        //            FfcParameterSet parameterSet = GetParameterSet(pqgMatch.Groups["parameterSet"].Value);
        //            HashFunction hashFunction = GetHashFunction(pqgMatch.Groups["hashFunction"].Value);
        //            var groupForMatch = 
        //                CreateGroupForMatch(
        //                    testType, 
        //                    scheme, 
        //                    kasMode, 
        //                    iutKasRole, 
        //                    iutKeyConfRole, 
        //                    iutKeyConfDirection, 
        //                    kdfType, 
        //                    pqgMatch, 
        //                    parameterSet, 
        //                    hashFunction,
        //                    fileName,
        //                    fileContent
        //                );


        //            groups.Add(groupForMatch.group);

        //            string testCasesRaw = groupForMatch.testCasesForGroup;
        //            var testCaseMatches = _testCaseRegex.Matches(testCasesRaw);

        //            foreach (Match testCaseMatch in testCaseMatches)
        //            {
        //                int testCaseId = int.Parse(testCaseMatch.Groups["COUNT"].Value.Trim());

        //                TestCase tc = new TestCase()
        //                {
        //                    TestCaseId = testCaseId,
        //                    StaticPrivateKeyServer =
        //                        GetBitStringFromString(testCaseMatch.Groups["XstatCAVS"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    StaticPublicKeyServer =
        //                        GetBitStringFromString(testCaseMatch.Groups["YstatCAVS"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    EphemeralPrivateKeyServer =
        //                        GetBitStringFromString(testCaseMatch.Groups["XephemCAVS"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    EphemeralPublicKeyServer =
        //                        GetBitStringFromString(testCaseMatch.Groups["YephemCAVS"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    StaticPrivateKeyIut =
        //                        GetBitStringFromString(testCaseMatch.Groups["XstatIUT"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    StaticPublicKeyIut =
        //                        GetBitStringFromString(testCaseMatch.Groups["YstatIUT"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    EphemeralPrivateKeyIut =
        //                        GetBitStringFromString(testCaseMatch.Groups["XephemIUT"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    EphemeralPublicKeyIut =
        //                        GetBitStringFromString(testCaseMatch.Groups["YephemIUT"].Value)
        //                            ?.ToPositiveBigInteger() ?? 0,
        //                    NonceAesCcm = GetBitStringFromString(testCaseMatch.Groups["YephemIUT"].Value),
        //                    OtherInfo = GetBitStringFromString(testCaseMatch.Groups["OI"].Value),
        //                    Tag = GetBitStringFromString(testCaseMatch.Groups["CAVSTag"].Value),
        //                    Z = GetBitStringFromString(testCaseMatch.Groups["Z"].Value),
        //                    MacData = GetBitStringFromString(testCaseMatch.Groups["MacData"].Value),
        //                    Dkm = GetBitStringFromString(testCaseMatch.Groups["DKM"].Value),
        //                    HashZ = GetBitStringFromString(testCaseMatch.Groups["CAVSHashZZ"].Value),
        //                    Result = testCaseMatch.Groups["Result"].Value
        //                };

        //                if (!string.IsNullOrEmpty(tc.Result))
        //                {
        //                    tc.FailureTest = tc.Result.StartsWith("F", StringComparison.OrdinalIgnoreCase);
        //                }

        //                groupForMatch.group.Tests.Add(tc);
        //            }

        //        }
        //    }

        //    TestVectorSet tv = new TestVectorSet()
        //    {
        //        Algorithm = Algorithm,
        //        TestGroups = groups.Select(g => (ITestGroup)g).ToList()
        //    };

        //    return new ParseResponse<TestVectorSet>(tv);
        //}

        //private (FfcParameterSet parameterSet, KeyAgreementMacType macType, int keyLength, int nonceLength, int tagLength) GetParameterSetAttributes(MatchCollection parameterSetMatches)
        //{
        //    var parameterSet = GetParameterSet(parameterSetMatches[0].Groups["parameterSet"].Value);
        //    var macTypes = GetMacTypes(parameterSetMatches[0]);

        //    return (
        //        parameterSet, 
        //        macTypes.macType, 
        //        macTypes.keyLength, 
        //        macTypes.nonceLength, 
        //        macTypes.tagLength
        //    );
        //}

        //private (KeyAgreementMacType macType, int keyLength, int nonceLength, int tagLength) GetMacTypes(Match value)
        //{
        //    KeyAgreementMacType macType = KeyAgreementMacType.None;
        //    int keyLength = int.Parse(_keySizeRegex.Match(value.Value).Groups["keySize"].Value.Trim());
        //    int nonceLength = 0;
        //    int tagLength = int.Parse(_tagLengthRegex.Match(value.Value).Groups["tagLength"].Value.Trim());

        //    // Mac Type
        //    var macTypeString = value.Groups["macType"].Value.ToLower().Trim();
        //    switch (macTypeString)
        //    {
        //        case "ccm":
        //            macType = KeyAgreementMacType.AesCcm;
        //            // values are in bytes, multiply by 8 for bits
        //            nonceLength = int.Parse(_nonceLengthRegex.Match(value.Value).Groups["nonceLength"].Value.Trim()) * 8;
        //            tagLength *= 8;
        //            break;
        //        case "cmac":
        //            macType = KeyAgreementMacType.CmacAes;
        //            break;
        //        case "hmac":
        //            var sha = _shaRegex.Match(value.Value).Groups["sha"].Value;
        //            var hashFunction = GetHashFunction(sha);
        //            switch (hashFunction.DigestSize)
        //            {
        //                case DigestSizes.d224:
        //                    macType = KeyAgreementMacType.HmacSha2D224;
        //                    break;
        //                case DigestSizes.d256:
        //                    macType = KeyAgreementMacType.HmacSha2D256;
        //                    break;
        //                case DigestSizes.d384:
        //                    macType = KeyAgreementMacType.HmacSha2D384;
        //                    break;
        //                case DigestSizes.d512:
        //                    macType = KeyAgreementMacType.HmacSha2D512;
        //                    break;
        //                default:
        //                    throw new ArgumentException($"cannot map {sha} to {nameof(macType)}");
        //            }
        //            break;
        //        default:
        //            throw new ArgumentException($"cannot map {macTypeString} to {nameof(macType)}");
        //    }

        //    return (
        //        macType,
        //        keyLength,
        //        nonceLength,
        //        tagLength
        //    );
        //}

        //private (TestGroup group, string testCasesForGroup) CreateGroupForMatch(string testType, FfcScheme scheme, KasMode kasMode, KeyAgreementRole iutKasRole, KeyConfirmationRole iutKeyConfRole, KeyConfirmationDirection iutKeyConfDirection, string kdfType, Match pqgMatch, FfcParameterSet parameterSet, HashFunction hashFunction, string fileName, string fileContent)
        //{
        //    TestGroup testGroup = new TestGroup()
        //    {
        //        KasMode = kasMode,
        //        Scheme = scheme,
        //        MacType = KeyAgreementMacType.None,
        //        KasRole = iutKasRole,
        //        Function = KasAssurance.DpGen | KasAssurance.DpVal | KasAssurance.FullVal |
        //                    KasAssurance.KeyPairGen | KasAssurance.KeyRegen,
        //        HashAlg = hashFunction,
        //        ParmSet = parameterSet,
        //        KcRole = iutKeyConfRole,
        //        KcType = iutKeyConfDirection,
        //        KdfType = kdfType,
        //        IdServer = _serverId,
        //        IdServerLen = _serverId.BitLength,
        //        IdIut = _iutId,
        //        IdIutLen = _iutId.BitLength,
        //        OiPattern = "uPartyInfo||vPartyInfo",
        //        P = GetBitStringFromString(pqgMatch.Groups["p"].Value)?.ToPositiveBigInteger() ?? 0,
        //        Q = GetBitStringFromString(pqgMatch.Groups["q"].Value)?.ToPositiveBigInteger() ?? 0,
        //        G = GetBitStringFromString(pqgMatch.Groups["g"].Value)?.ToPositiveBigInteger() ?? 0,
        //        TestType = testType,
        //        Tests = new List<ITestCase>()
        //    };

        //    // The "parameter attributes" only matter to KDF/KC
        //    if (kasMode != KasMode.NoKdfNoKc)
        //    {
        //        var parameterSetMatches = _parameterSetRegex.Matches(fileContent);
        //        var parameterSetAttributes = GetParameterSetAttributes(parameterSetMatches);

        //        testGroup.MacType = parameterSetAttributes.macType;
        //        testGroup.KeyLen = parameterSetAttributes.keyLength;
        //        testGroup.AesCcmNonceLen = parameterSetAttributes.nonceLength;
        //        testGroup.MacLen = parameterSetAttributes.tagLength;
        //    }

        //    string testCases = pqgMatch.Groups["remaining"].Value;

        //    return (testGroup, testCases);
        //}

        private void GetKasOptionsFromFileName(string fileName, ref FfcScheme scheme, ref KasMode kasMode, ref KeyAgreementRole iutKasRole, ref KeyConfirmationRole iutKeyConfRole, ref KeyConfirmationDirection iutKeyConfDirection, ref string kdfType)
        {
            GetScheme(fileName, ref scheme);
            GetKasMode(fileName, ref kasMode);
            GetIutKasRole(fileName, ref iutKasRole);
            GetKeyConfirmationInformation(fileName, kasMode, ref iutKeyConfRole, ref iutKeyConfDirection);
            GetKdfType(fileName, ref kdfType);
        }

        private void GetScheme(string fileName, ref FfcScheme scheme)
        {
            if (fileName.Contains("ffcephem"))
            {
                scheme = FfcScheme.DhEphem;
                return;
            }

            throw new ArgumentException($"cannot map {fileName} to {nameof(scheme)}");
        }

        private void GetKasMode(string fileName, ref KasMode kasMode)
        {
            if (fileName.Contains("_zzonly_"))
            {
                kasMode = KasMode.NoKdfNoKc;
                return;
            }
            if (fileName.Contains("_nokc_"))
            {
                kasMode = KasMode.KdfNoKc;
                return;
            }
            if (fileName.Contains("_kc_"))
            {
                kasMode = KasMode.KdfKc;
                return;
            }

            throw new ArgumentException($"cannot map {fileName} to {nameof(kasMode)}");
        }

        private void GetIutKasRole(string fileName, ref KeyAgreementRole iutKasRole)
        {
            if (fileName.Contains("_init"))
            {
                iutKasRole = KeyAgreementRole.InitiatorPartyU;
                return;
            }
            if (fileName.Contains("_resp"))
            {
                iutKasRole = KeyAgreementRole.ResponderPartyV;
                return;
            }

            throw new ArgumentException($"cannot map {fileName} to {nameof(iutKasRole)}");
        }

        private void GetKeyConfirmationInformation(string fileName, KasMode kasMode, ref KeyConfirmationRole iutKeyConfRole, ref KeyConfirmationDirection iutKeyConfDirection)
        {
            if (kasMode != KasMode.KdfKc)
            {
                iutKeyConfRole = KeyConfirmationRole.None;
                iutKeyConfDirection = KeyConfirmationDirection.None;
                return;
            }

            if (fileName.Contains("_prov_"))
            {
                iutKeyConfRole = KeyConfirmationRole.Provider;
            }
            else if (fileName.Contains("_rcpt_"))
            {
                iutKeyConfRole = KeyConfirmationRole.Recipient;
            }
            else
            {
                throw new ArgumentException($"cannot map {fileName} to {nameof(iutKeyConfRole)}");
            }

            if (fileName.Contains("_ulat"))
            {
                iutKeyConfDirection = KeyConfirmationDirection.Unilateral;
            }
            else if (fileName.Contains("_blat"))
            {
                iutKeyConfDirection = KeyConfirmationDirection.Bilateral;
            }
            else
            {
                throw new ArgumentException($"cannot map {fileName} to {nameof(iutKeyConfDirection)}");
            }
        }

        private void GetKdfType(string fileName, ref string kdfType)
        {
            if (fileName.Contains("_kdfasn1"))
            {
                kdfType = "asn1";
                return;
            }

            if (fileName.Contains("_kdfconcat"))
            {
                kdfType = "concatenation";
                return;
            }
        }

        private FfcParameterSet GetParameterSet(string parameterSetString)
        {
            return EnumHelpers.GetEnumFromEnumDescription<FfcParameterSet>(parameterSetString.ToLower().Trim());
        }

        private HashFunction GetHashFunction(string hashFunctionString)
        {
            hashFunctionString = hashFunctionString.Trim().ToLower();

            if (hashFunctionString.Equals("sha224"))
            {
                return new HashFunction(ModeValues.SHA2, DigestSizes.d224);
            }
            if (hashFunctionString.Equals("sha256"))
            {
                return new HashFunction(ModeValues.SHA2, DigestSizes.d256);
            }
            if (hashFunctionString.Equals("sha384"))
            {
                return new HashFunction(ModeValues.SHA2, DigestSizes.d384);
            }
            if (hashFunctionString.Equals("sha512"))
            {
                return new HashFunction(ModeValues.SHA2, DigestSizes.d512);
            }

            throw new ArgumentException($"cannot map {hashFunctionString} to {nameof(HashFunction)}");
        }

        //private BitString GetBitStringFromString(string value)
        //{
        //    value = value.Trim();
        //    if (string.IsNullOrEmpty(value))
        //    {
        //        return null;
        //    }

        //    return new BitString(value);
        //}

    }
}
