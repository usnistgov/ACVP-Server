using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Libraries.Math;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC.Parsers
{
    /// <summary>
    /// Note that KAS files are exceedingly complex, 
    /// limiting usage of a single MAC/parameter set will save time on parsing
    /// </summary>
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
    {
        private const string Algorithm = "KAS-FFC";
        private readonly BitString _serverId = new BitString("434156536964");
        private readonly BitString _iutId = new BitString("a1b2c3d4e5");
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ParseResponse<TestVectorSet> Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                var thereWasNoPathSupplied = "There was no path supplied.";
                _logger.Error($"{thereWasNoPathSupplied}");
                return new ParseResponse<TestVectorSet>(thereWasNoPathSupplied);
            }

            if (!Directory.Exists(path))
            {
                var notFound = $"Could not find path {path}";
                _logger.Error(notFound);
                return new ParseResponse<TestVectorSet>(notFound);
            }

            var groups = new List<TestGroup>();

            var files = Directory.GetFiles(path, "*FFC*.fax", SearchOption.AllDirectories);
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
                    _logger.Error(ex, ex.Message);
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
                    Tests = new List<TestCase>(),
                    OiPattern = "uPartyInfo||vPartyInfo",
                    Function = KasAssurance.DpGen | KasAssurance.DpVal | KasAssurance.FullVal | KasAssurance.KeyPairGen | KasAssurance.KeyRegen,
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
                //int caseId = -1;
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
                    string hmacMacAlgoLine = "[HMAC SHAs supported:";
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
                TestGroups = groups.Select(g => g).ToList()
            };
            return new ParseResponse<TestVectorSet>(testVectorSet);
        }

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
            if (fileName.Contains("ffchybrid1"))
            {
                scheme = FfcScheme.DhHybrid1;
                return;
            }
            if (fileName.Contains("ffchybridoneflow"))
            {
                scheme = FfcScheme.DhHybridOneFlow;
                return;
            }
            if (fileName.Contains("ffconeflow"))
            {
                scheme = FfcScheme.DhOneFlow;
                return;
            }
            if (fileName.Contains("ffcstatic"))
            {
                scheme = FfcScheme.DhStatic;
                return;
            }
            if (fileName.Contains("ffcmqv1"))
            {
                scheme = FfcScheme.Mqv1;
                return;
            }
            if (fileName.Contains("ffcmqv2"))
            {
                scheme = FfcScheme.Mqv2;
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
    }
}
