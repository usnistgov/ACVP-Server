using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private readonly string[] _testTypes = new string[] { "AFT", "VAL" };
        private readonly IDsaFfcFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;

        public TestGroupGenerator(IDsaFfcFactory dsaFactory, IShaFactory shaFactory)
        {
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
        }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            var flagFunctions = SpecificationMapping.FunctionArrayToFlags(parameters.Function);
            GenerateGroups(parameters.Scheme, flagFunctions, groups);

            GeneratePqgPerGroup(groups);
            
            return groups;
        }

        private void GenerateGroups(Schemes parametersScheme, KasAssurance flagFunctions, List<TestGroup> groups)
        {
            CreateGroupsPerScheme(parametersScheme.DhEphem, flagFunctions, groups);
            // TODO additional schemes
        }

        private void GeneratePqgPerGroup(List<TestGroup> groups)
        {
            foreach (var group in groups)
            {
                var shaAttributes = ShaAttributes.GetShaAttributes(group.HashAlg.Mode, group.HashAlg.DigestSize);

                var dsa = _dsaFactory.GetInstance(group.HashAlg, EntropyProviderTypes.Random);
                var parameterSetAttributes = FfcParameterSetDetails.GetDetailsForParameterSet(group.ParmSet);

                var domainParams = dsa.GenerateDomainParameters(
                    new FfcDomainParametersGenerateRequest(
                        parameterSetAttributes.qLength,
                        parameterSetAttributes.pLength,
                        parameterSetAttributes.qLength,
                        shaAttributes.outputLen,
                        null,
                        PrimeGenMode.None, 
                        GeneratorGenMode.Unverifiable
                    )
                );

                group.P = domainParams.PqgDomainParameters.P;
                group.Q = domainParams.PqgDomainParameters.Q;
                group.G = domainParams.PqgDomainParameters.G;
            }
        }

        private void CreateGroupsPerScheme(SchemeBase schemeBase, KasAssurance flagFunctions, List<TestGroup> groups)
        {
            if (schemeBase == null)
            {
                return;
            }

            FfcScheme scheme = SpecificationMapping.GetEnumFromType(schemeBase);

            CreateGroupsForNoKdfNoKc(schemeBase, flagFunctions, scheme, groups);
            CreateGroupsForKdfNoKc(schemeBase, flagFunctions, scheme, groups);
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
                foreach (var role in schemeBase.Role)
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
                        (GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Fb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Fb)
                            .OrderBy(ob => Guid.NewGuid())
                            .ToList())
                    },
                    {
                        FfcParameterSet.Fc,
                        (GetHashAlgsPerParameterSet(schemeBase.NoKdfNoKc.ParameterSet.Fc).OrderBy(ob => Guid.NewGuid())
                            .ToList(),
                        GetMacAlgsPerParameterSet(schemeBase.KdfNoKc.ParameterSet.Fc)
                            .OrderBy(ob => Guid.NewGuid()).ToList())
                    }
                };

            foreach (var testType in _testTypes)
            {
                foreach (var role in schemeBase.Role)
                {
                    foreach (var kdf in GetKdfOptions(schemeBase.KdfNoKc.KdfOption))
                    {
                        foreach (var parameterSet in hashPerParameterSet)
                        {
                            if (parameterSet.Value.hashFunc.Any())
                            {
                                var mac = parameterSet.Value.mac.OrderBy(ob => Guid.NewGuid()).ToList().First();
                                var keyLen = mac.KeyLen.OrderBy(ob => Guid.NewGuid()).ToList().First();
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