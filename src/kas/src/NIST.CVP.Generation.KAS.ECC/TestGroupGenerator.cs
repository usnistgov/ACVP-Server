using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const int MAX_KEY_SIZE = 4096;
        private readonly string[] _testTypes = new string[] { "AFT", "VAL" };
        
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            var flagFunctions = SpecificationMapping.FunctionArrayToFlags(parameters.Function);
            GenerateGroups(parameters.Scheme, flagFunctions, groups);

            return groups;
        }

        private void GenerateGroups(Schemes parametersScheme, KasAssurance flagFunctions, List<TestGroup> groups)
        {
            CreateGroupsPerScheme(parametersScheme.EccFullUnified, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.EccFullMqv, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.EccEphemeralUnified, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.EccOnePassUnified, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.EccOnePassMqv, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.EccOnePassDh, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.EccStaticUnified, flagFunctions, groups);
        }

        private void CreateGroupsPerScheme(SchemeBase schemeBase, KasAssurance flagFunctions, List<TestGroup> groups)
        {
            if (schemeBase == null)
            {
                return;
            }

            EccScheme scheme = SpecificationMapping.GetEccEnumFromType(schemeBase);

            CreateGroupsForNoKdfNoKc(schemeBase, flagFunctions, scheme, groups);
            CreateGroupsForKdfNoKc(schemeBase, flagFunctions, scheme, groups);
            CreateGroupsForKdfKc(schemeBase, flagFunctions, scheme, groups);
        }

        #region adds test groups
        private void CreateGroupsForNoKdfNoKc(SchemeBase schemeBase, KasAssurance flagFunctions, EccScheme scheme, List<TestGroup> groups)
        {
            if (schemeBase.NoKdfNoKc == null)
            {
                return;
            }

            Dictionary<EccParameterSet, (List<HashFunction> hashFunctions, string curveName)> hashPerParameterSet =
                new Dictionary<EccParameterSet, (List<HashFunction> hashFunctions, string curveName)>
                {
                    {
                        EccParameterSet.Eb,
                        (GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Eb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        schemeBase.NoKdfNoKc.ParameterSet.Eb?.Curve)
                    },
                    {
                        EccParameterSet.Ec,
                        (GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Ec)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        schemeBase.NoKdfNoKc.ParameterSet.Ec?.Curve)
                    },
                    {
                        EccParameterSet.Ed,
                        (GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Ed)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        schemeBase.NoKdfNoKc.ParameterSet.Ed?.Curve)
                    },
                    {
                        EccParameterSet.Ee,
                        (GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Ee)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        schemeBase.NoKdfNoKc.ParameterSet.Ee?.Curve)
                    }
                };

            foreach (var testType in _testTypes)
            {
                foreach (var role in schemeBase.KasRole)
                {
                    foreach (var parameterSet in hashPerParameterSet)
                    {
                        if (parameterSet.Value.hashFunctions.Any())
                        {
                            groups.Add(new TestGroup()
                            {
                                Scheme = scheme,
                                Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(parameterSet.Value.curveName),
                                KasMode = KasMode.NoKdfNoKc,
                                TestType = testType,
                                Function = flagFunctions,
                                HashAlg = parameterSet.Value.hashFunctions.First(),
                                KasRole = EnumHelpers.GetEnumFromEnumDescription<KeyAgreementRole>(role),
                                ParmSet = parameterSet.Key
                            });
                        }
                    }
                }
            }
        }

        private void CreateGroupsForKdfNoKc(SchemeBase schemeBase, KasAssurance flagFunctions, EccScheme scheme, List<TestGroup> groups)
        {
            if (schemeBase.KdfNoKc == null)
            {
                return;
            }

            Dictionary<EccParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac, string curveName)> hashPerParameterSet =
                new Dictionary<EccParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac, string curveName)>
                {
                    {
                        EccParameterSet.Eb,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Eb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Eb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        schemeBase.KdfNoKc.ParameterSet.Eb?.Curve)
                    },
                    {
                        EccParameterSet.Ec,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Ec).OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Ec)
                            .OrderBy(ob => Guid.NewGuid()).ToList(),
                        schemeBase.KdfNoKc.ParameterSet.Ec?.Curve)
                    },
                    {
                        EccParameterSet.Ed,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Ed).OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Ed)
                            .OrderBy(ob => Guid.NewGuid()).ToList(),
                        schemeBase.KdfNoKc.ParameterSet.Ed?.Curve)
                    },
                    {
                        EccParameterSet.Ee,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Ee).OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Ee)
                            .OrderBy(ob => Guid.NewGuid()).ToList(),
                        schemeBase.KdfNoKc.ParameterSet.Ee?.Curve)
                    }
                };

            foreach (var testType in _testTypes)
            {
                foreach (var role in schemeBase.KasRole)
                {
                    foreach (var kdf in GetKdfOptions(schemeBase.KdfNoKc.KdfOption))
                    {
                        foreach (var parameterSet in hashPerParameterSet)
                        {
                            if (parameterSet.Value.hashFunc.Any() && parameterSet.Value.mac.Any())
                            {
                                var mac = parameterSet.Value.mac.OrderBy(ob => Guid.NewGuid()).ToList().First();
                                mac.KeyLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
                                mac.KeyLen.SetMaximumAllowedValue(MAX_KEY_SIZE);
                                var keyLen = mac.KeyLen.GetValues(1).OrderBy(ob => Guid.NewGuid()).ToList().First();
                                var keyAgreementMacType =
                                    SpecificationMapping.GetMacInfoFromParameterClass(mac).keyAgreementMacType;

                                groups.Add(new TestGroup()
                                {
                                    Scheme = scheme,
                                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(parameterSet.Value.curveName),
                                    KasMode = KasMode.KdfNoKc,
                                    TestType = testType,
                                    Function = flagFunctions,
                                    HashAlg = parameterSet.Value.hashFunc.First(),
                                    KasRole = EnumHelpers.GetEnumFromEnumDescription<KeyAgreementRole>(role),
                                    ParmSet = parameterSet.Key,
                                    KdfType = kdf.Key,
                                    OiPattern = kdf.Value,
                                    MacType = keyAgreementMacType,
                                    KeyLen = keyLen,
                                    MacLen = mac.MacLen,
                                    AesCcmNonceLen = mac.NonceLen
                                });
                            }
                        }
                    }
                }
            }
        }

        private void CreateGroupsForKdfKc(SchemeBase schemeBase, KasAssurance flagFunctions, EccScheme scheme, List<TestGroup> groups)
        {
            if (schemeBase.KdfKc == null)
            {
                return;
            }

            Dictionary<EccParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac, string curveName)> hashPerParameterSet =
                new Dictionary<EccParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac, string curveName)>
                {
                    {
                        EccParameterSet.Eb,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Eb)
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Eb)
                            .ToList(),
                        schemeBase.KdfKc.ParameterSet.Eb?.Curve)
                    },
                    {
                        EccParameterSet.Ec,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Ec).ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Ec).ToList(),
                        schemeBase.KdfKc.ParameterSet.Ec?.Curve)
                    },
                    {
                        EccParameterSet.Ed,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Ed).ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Ed).ToList(),
                        schemeBase.KdfKc.ParameterSet.Ed?.Curve)
                    },
                    {
                        EccParameterSet.Ee,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Ee).ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Ee).ToList(),
                        schemeBase.KdfKc.ParameterSet.Ee?.Curve)
                    }
                };

            foreach (var testType in _testTypes)
            {
                foreach (var role in schemeBase.KasRole)
                {
                    foreach (var kdf in GetKdfOptions(schemeBase.KdfKc.KdfOption))
                    {
                        foreach (var nonceType in schemeBase.KdfKc.KcOption.NonceType)
                        {
                            foreach (var kcRole in schemeBase.KdfKc.KcOption.KcRole)
                            {
                                foreach (var kcType in schemeBase.KdfKc.KcOption.KcType)
                                {
                                    foreach (var parameterSet in hashPerParameterSet)
                                    {
                                        if (parameterSet.Value.hashFunc.Any() && parameterSet.Value.mac.Any())
                                        {
                                            foreach (var mac in parameterSet.Value.mac)
                                            {
                                                mac.KeyLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
                                                mac.KeyLen.SetMaximumAllowedValue(MAX_KEY_SIZE);

                                                foreach (var keyLen in mac.KeyLen.GetValues(1).OrderBy(ob => Guid.NewGuid()).Take(1))
                                                {
                                                    var keyAgreementMacType =
                                                        SpecificationMapping.GetMacInfoFromParameterClass(mac)
                                                            .keyAgreementMacType;

                                                    var curve = 
                                                        EnumHelpers.GetEnumFromEnumDescription<Curve>(parameterSet.Value.curveName);
                                                    var kasRoleEnum =
                                                        EnumHelpers.GetEnumFromEnumDescription<KeyAgreementRole>(role);
                                                    var kcRoleEnum =
                                                        EnumHelpers.GetEnumFromEnumDescription<KeyConfirmationRole>(kcRole);
                                                    var kcTypeEnum =
                                                        EnumHelpers.GetEnumFromEnumDescription<KeyConfirmationDirection>(kcType);

                                                    // DhOneFlow only allows unilateral key confirmation V to U
                                                    // do not create groups outside of that constraint
                                                    if (scheme == EccScheme.OnePassDh)
                                                    {
                                                        if (kcTypeEnum == KeyConfirmationDirection.Bilateral ||
                                                            (kasRoleEnum == KeyAgreementRole.InitiatorPartyU &&
                                                             kcRoleEnum == KeyConfirmationRole.Provider) ||
                                                            (kasRoleEnum == KeyAgreementRole.ResponderPartyV &&
                                                             kcRoleEnum == KeyConfirmationRole.Recipient))
                                                        {
                                                            continue;
                                                        }
                                                    }

                                                    groups.Add(new TestGroup()
                                                    {
                                                        Scheme = scheme,
                                                        Curve = curve,
                                                        KasMode = KasMode.KdfKc,
                                                        TestType = testType,
                                                        Function = flagFunctions,
                                                        HashAlg = parameterSet.Value.hashFunc.First(),
                                                        KasRole = kasRoleEnum,
                                                        KcRole = kcRoleEnum,
                                                        KcType = kcTypeEnum,
                                                        NonceType = nonceType,
                                                        ParmSet = parameterSet.Key,
                                                        KdfType = kdf.Key,
                                                        OiPattern = kdf.Value,
                                                        MacType = keyAgreementMacType,
                                                        KeyLen = keyLen,
                                                        MacLen = mac.MacLen,
                                                        AesCcmNonceLen = mac.NonceLen
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion adds test groups

        private IEnumerable<HashFunction> GetHashAlgsPerParameterSet(ParameterSetBase parameterSet)
        {
            List<HashFunction> hashAlgs = new List<HashFunction>();

            if (parameterSet != null)
            {
                foreach (var hashAlg in parameterSet.HashAlg)
                {
                    var attribute = ShaAttributes.GetShaAttributes(hashAlg);
                    hashAlgs.Add(new HashFunction(attribute.mode, attribute.digestSize));
                }
            }

            return hashAlgs;
        }

        private IEnumerable<MacOptionsBase> GetMacAlgsPerParameterSet(ParameterSetBase parameterSet)
        {
            List<MacOptionsBase> macOptions = new List<MacOptionsBase>();

            if (parameterSet != null)
            {
                macOptions.AddIfNotNull(parameterSet.MacOption.AesCcm);
                macOptions.AddIfNotNull(parameterSet.MacOption.Cmac);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha2_D224);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha2_D256);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha2_D384);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha2_D512);
            }

            return macOptions;
        }

        private Dictionary<string, string> GetKdfOptions(KdfOptions kdfOptions)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(kdfOptions.Asn1))
            {
                dict.Add("asn1", kdfOptions.Asn1);
            }
            if (!string.IsNullOrEmpty(kdfOptions.Concatenation))
            {
                dict.Add("concatenation", kdfOptions.Concatenation);
            }

            return dict;
        }
    }
}