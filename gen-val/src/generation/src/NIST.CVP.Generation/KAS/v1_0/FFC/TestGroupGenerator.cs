using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const int MAX_KEY_SIZE = 4096;
        private readonly string[] _testTypes = new string[] { "AFT", "VAL" };
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            var flagFunctions = SpecificationMapping.FunctionArrayToFlags(parameters.Function);
            GenerateGroups(parameters.Scheme, flagFunctions, groups);

            await GeneratePqgPerGroupAsync(groups);
            
            return groups;
        }

        private void GenerateGroups(Schemes parametersScheme, KasAssurance flagFunctions, List<TestGroup> groups)
        {
            CreateGroupsPerScheme(parametersScheme.FfcDhHybrid1, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.FfcMqv2, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.FfcDhEphem, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.FfcDhHybridOneFlow, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.FfcMqv1, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.FfcDhOneFlow, flagFunctions, groups);
            CreateGroupsPerScheme(parametersScheme.FfcDhStatic, flagFunctions, groups);
        }

        private async Task GeneratePqgPerGroupAsync(List<TestGroup> groups)
        {
            // Get collection of individual permutations of L/N/Hash for PQG creation
            // Doing it this way as there will likely be overlap between l/n/hashFunc
            // and no reason to generate a new PQG for each individual instance
            var perms = new HashSet<(int l, int n, HashFunction hashFunc)>();
            foreach (var group in groups)
            {
                var parameterSetAttributes = ParameterSetDetails.GetDetailsForFfcParameterSet(group.ParmSet);
                perms.Add((parameterSetAttributes.pLength, parameterSetAttributes.qLength, group.HashAlg));
            }

            // Get PQG for each permutation
            var pqgTasks = new Dictionary<(int l, int n, HashFunction hashFunc), Task<DsaDomainParametersResult>>();
            foreach (var perm in perms)
            {
                pqgTasks.Add(perm, _oracle.GetDsaDomainParametersAsync(new DsaDomainParametersParameters()
                {
                    GGenMode = GeneratorGenMode.Unverifiable,
                    HashAlg = perm.hashFunc,
                    L = perm.l,
                    N = perm.n,
                    PQGenMode = PrimeGenMode.Probable
                }));
            }

            // Assign the groups to the appropriate PQG
            foreach (var group in groups)
            {
                var parameterSetAttributes = ParameterSetDetails.GetDetailsForFfcParameterSet(group.ParmSet);
                var pqgTask = pqgTasks
                    .First(w => w.Key.l == parameterSetAttributes.pLength &&
                                w.Key.n == parameterSetAttributes.qLength &&
                                w.Key.hashFunc.Equals(group.HashAlg)).Value;

                var pqg = await pqgTask;

                group.L = parameterSetAttributes.pLength;
                group.N = parameterSetAttributes.qLength;

                group.DomainParams = new FfcDomainParameters(pqg.P, pqg.Q, pqg.G);
            }
        }

        private void CreateGroupsPerScheme(SchemeBase schemeBase, KasAssurance flagFunctions, List<TestGroup> groups)
        {
            if (schemeBase == null)
            {
                return;
            }

            FfcScheme scheme = SpecificationMapping.GetFfcEnumFromType(schemeBase);

            CreateGroupsForNoKdfNoKc(schemeBase, flagFunctions, scheme, groups);
            CreateGroupsForKdfNoKc(schemeBase, flagFunctions, scheme, groups);
            CreateGroupsForKdfKc(schemeBase, flagFunctions, scheme, groups);
        }

        #region adds test groups
        private void CreateGroupsForNoKdfNoKc(SchemeBase schemeBase, KasAssurance flagFunctions, FfcScheme scheme, List<TestGroup> groups)
        {
            if (schemeBase.NoKdfNoKc == null)
            {
                return;
            }

            Dictionary<FfcParameterSet, List<HashFunction>> hashPerParameterSet =
                new Dictionary<FfcParameterSet, List<HashFunction>>
                {
                    {
                        FfcParameterSet.Fb,
                        GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Fb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList()
                    },
                    {
                        FfcParameterSet.Fc,
                        GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Fc)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList()
                    }
                };

            foreach (var testType in _testTypes)
            {
                foreach (var role in schemeBase.KasRole)
                {
                    foreach (var parameterSet in hashPerParameterSet)
                    {
                        if (parameterSet.Value.Any())
                        {
                            groups.Add(new TestGroup()
                            {
                                Scheme = scheme,
                                KasMode = KasMode.NoKdfNoKc,
                                TestType = testType,
                                Function = flagFunctions,
                                HashAlg = parameterSet.Value.First(),
                                KasRole = EnumHelpers.GetEnumFromEnumDescription<KeyAgreementRole>(role),
                                ParmSet = parameterSet.Key
                            });
                        }
                    }
                }
            }
        }

        private void CreateGroupsForKdfNoKc(SchemeBase schemeBase, KasAssurance flagFunctions, FfcScheme scheme, List<TestGroup> groups)
        {
            if (schemeBase.KdfNoKc == null)
            {
                return;
            }

            Dictionary<FfcParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac)> hashPerParameterSet =
                new Dictionary<FfcParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac)>
                {
                    {
                        FfcParameterSet.Fb,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Fb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Fb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList())
                    },
                    {
                        FfcParameterSet.Fc,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Fc).OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Fc)
                            .OrderBy(ob => Guid.NewGuid()).ToList())
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

        private void CreateGroupsForKdfKc(SchemeBase schemeBase, KasAssurance flagFunctions, FfcScheme scheme, List<TestGroup> groups)
        {
            if (schemeBase.KdfKc == null)
            {
                return;
            }

            Dictionary<FfcParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac)> hashPerParameterSet =
                new Dictionary<FfcParameterSet, (List<HashFunction> hashFunc, List<MacOptionsBase> mac)>
                {
                    {
                        FfcParameterSet.Fb,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Fb)
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Fb)
                            .ToList())
                    },
                    {
                        FfcParameterSet.Fc,
                        (GetHashAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Fc).ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfKc.ParameterSet.Fc).ToList())
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

                                                    var kasRoleEnum =
                                                        EnumHelpers.GetEnumFromEnumDescription<KeyAgreementRole>(role);
                                                    var kcRoleEnum =
                                                        EnumHelpers.GetEnumFromEnumDescription<KeyConfirmationRole>(
                                                            kcRole);
                                                    var kcTypeEnum =
                                                        EnumHelpers
                                                            .GetEnumFromEnumDescription<KeyConfirmationDirection>(
                                                                kcType);

                                                    // DhOneFlow only allows unilateral key confirmation V to U
                                                    // do not create groups outside of that constraint
                                                    if (scheme == FfcScheme.DhOneFlow)
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
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha2_D512_T224);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha2_D512_T256);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha3_D224);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha3_D256);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha3_D384);
                macOptions.AddIfNotNull(parameterSet.MacOption.HmacSha3_D512);
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