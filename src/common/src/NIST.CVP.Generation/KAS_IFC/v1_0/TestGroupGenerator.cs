using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private static readonly string[] TestTypes = {"AFT", "VAL"};
        
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            GenerateGroups(parameters.Scheme, groups);

            return groups;
        }

        private void GenerateGroups(Schemes parametersScheme, List<TestGroup> groups)
        {
            foreach (var scheme in parametersScheme.GetRegisteredSchemes())
            {
                CreateGroupsPerScheme(scheme, groups);
            }
        }

        private void CreateGroupsPerScheme(SchemeBase schemeBase, List<TestGroup> groups)
        {
            if (schemeBase == null)
            {
                return;
            }

            var keyGenMethods = schemeBase.KeyGenerationMethods.GetRegisteredKeyGenerationMethods().ToList();
            var macMethods = GetMacConfigurations(schemeBase.MacMethods);
            var isMacScheme = macMethods.Count() != 0;
            var ktsMethod = GetKtsConfigurations(schemeBase.KtsMethod);
            var kdfMethods = GetKdfConfigurations(schemeBase.KdfMethods);

            foreach (var testType in TestTypes)
            {
                foreach (var role in schemeBase.KasRole)
                {
                    foreach (var keyGenerationMethod in keyGenMethods)
                    {
                        foreach (var modulo in keyGenerationMethod.Modulo)
                        {
                            foreach (var kdfConfig in kdfMethods)
                            {
                                if (isMacScheme)
                                {
                                    foreach (var macMethod in macMethods)
                                    {
                                        groups.Add(new TestGroup()
                                        {
                                            L = schemeBase.L,
                                            Modulo = modulo,
                                            Scheme = schemeBase.Scheme,
                                            KasRole = role,
                                            KdfConfiguration = kdfConfig,
                                            KtsConfiguration = null,
                                            MacConfiguration = macMethod,
                                            TestType = testType,
                                            KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                        });
                                    }
                                }
                                else
                                {
                                    groups.Add(new TestGroup()
                                    {
                                        L = schemeBase.L,
                                        Modulo = modulo,
                                        Scheme = schemeBase.Scheme,
                                        KasRole = role,
                                        KdfConfiguration = kdfConfig,
                                        KtsConfiguration = null,
                                        MacConfiguration = null,
                                        TestType = testType,
                                        KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                    });
                                }
                            }

                            foreach (var ktsConfig in ktsMethod)
                            {
                                if (isMacScheme)
                                {
                                    foreach (var macMethod in macMethods)
                                    {
                                        groups.Add(new TestGroup()
                                        {
                                            L = schemeBase.L,
                                            Modulo = modulo,
                                            Scheme = schemeBase.Scheme,
                                            KasRole = role,
                                            KdfConfiguration = null,
                                            KtsConfiguration = ktsConfig,
                                            MacConfiguration = macMethod,
                                            TestType = testType,
                                            KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                        });
                                    }
                                }
                                else
                                {
                                    groups.Add(new TestGroup()
                                    {
                                        L = schemeBase.L,
                                        Modulo = modulo,
                                        Scheme = schemeBase.Scheme,
                                        KasRole = role,
                                        KdfConfiguration = null,
                                        KtsConfiguration = ktsConfig,
                                        MacConfiguration = null,
                                        TestType = testType,
                                        KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                    });
                                }
                            }
                        }
                    }
                }                
            }
        }

        private List<MacConfiguration> GetMacConfigurations(MacMethods schemeBaseMacMethods)
        {
            if (schemeBaseMacMethods == null)
            {
                return new List<MacConfiguration>();
            }
            
            List<MacConfiguration> list = new List<MacConfiguration>();

            foreach (var macMethod in schemeBaseMacMethods.GetRegisteredMacMethods())
            {
                list.Add(new MacConfiguration()
                {
                    MacType = macMethod.MacType,
                    KeyLen = macMethod.KeyLen,
                    MacLen = macMethod.MacLen
                });
            }
            
            return list;
        }

        private List<IKasKdfConfiguration> GetKdfConfigurations(KdfMethods kdfMethods)
        {
            var list = new List<IKasKdfConfiguration>();

            GetKdfConfiguration(kdfMethods.OneStepKdf, list);
            
            return list;
        }

        private void GetKdfConfiguration(OneStepKdf kdfMethodsOneStepKdf, List<IKasKdfConfiguration> list)
        {
            if (kdfMethodsOneStepKdf == null)
            {
                return;
            }

            foreach (var encoding in kdfMethodsOneStepKdf.Encoding)
            {
                foreach (var auxFunction in kdfMethodsOneStepKdf.AuxFunctions)
                {
                    foreach (var saltMethod in auxFunction.MacSaltMethods)
                    {
                        list.Add(new OneStepConfiguration()
                        {
                            Encoding = encoding,
                            AuxFunction = new Crypto.Common.KAS.KDF.KdfOneStep.AuxFunction()
                            {
                                SaltLen = auxFunction.SaltLen,
                                AuxFunctionName = auxFunction.AuxFunctionName,
                                FixedInputPattern = auxFunction.FixedInputPattern,
                                MacSaltMethod = saltMethod
                            }
                        });
                    }
                }
            }
        }
        
        private List<KtsConfiguration> GetKtsConfigurations(KtsMethod schemeBaseKtsMethod)
        {
            if (schemeBaseKtsMethod == null)
            {
                return new List<KtsConfiguration>();
            }
            
            var list = new List<KtsConfiguration>();
            
            foreach (var hashAlg in schemeBaseKtsMethod.HashAlgs)
            {
                if (!string.IsNullOrEmpty(schemeBaseKtsMethod.AssociatedDataPattern))
                {
                    list.Add(new KtsConfiguration()
                    {
                        AssociatedDataPattern = schemeBaseKtsMethod.AssociatedDataPattern,
                        KtsHashAlg = hashAlg
                    });                    
                }

                if (schemeBaseKtsMethod.SupportsNullAssociatedData)
                {
                    list.Add(new KtsConfiguration()
                    {
                        AssociatedDataPattern = string.Empty,
                        KtsHashAlg = hashAlg
                    });
                }
            }
            
            return list;
        }
    }
}