using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private static readonly string[] TestTypes =
        {
            "AFT", 
            // todo "VAL"
        };
        private static readonly BigInteger DefaultExponent = BigInteger.Zero; // new BigInteger(65537);
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            GenerateGroups(parameters.Scheme, parameters, groups);

            return groups;
        }

        private void GenerateGroups(Schemes parametersScheme, Parameters param, List<TestGroup> groups)
        {
            foreach (var scheme in parametersScheme.GetRegisteredSchemes())
            {
                CreateGroupsPerScheme(scheme, param, groups);
            }
        }

        private void CreateGroupsPerScheme(SchemeBase schemeBase, Parameters param, List<TestGroup> groups)
        {
            if (schemeBase == null)
            {
                return;
            }

            var keyGenMethods = schemeBase.KeyGenerationMethods.GetRegisteredKeyGenerationMethods().ToList();
            var macMethods = GetMacConfigurations(schemeBase.MacMethods);
            var isMacScheme = macMethods.Count() != 0;
            var ktsMethod = GetKtsConfigurations(schemeBase.KtsMethod);
            var kdfMethods = GetKdfConfigurations(schemeBase.KdfMethods, schemeBase.L);

            foreach (var testType in TestTypes)
            {
                var iutKeys = testType.Equals("AFT", StringComparison.OrdinalIgnoreCase) ? GetKeys(param) : null;
                
                foreach (var role in schemeBase.KasRole)
                {
                    var keyConfInfo = GetKeyConfirmationInfo(schemeBase.Scheme, role);
                    
                    foreach (var keyGenerationMethod in keyGenMethods)
                    {
                        var exponent = keyGenerationMethod.FixedPublicExponent == 0
                            ? DefaultExponent
                            : keyGenerationMethod.FixedPublicExponent;
                        
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
                                            IsSample = param.IsSample,
                                            L = schemeBase.L,
                                            Modulo = modulo,
                                            PublicExponent = exponent,
                                            Scheme = schemeBase.Scheme,
                                            KasMode = schemeBase.KasMode,
                                            KasRole = role,
                                            KdfConfiguration = kdfConfig,
                                            KtsConfiguration = null,
                                            MacConfiguration = macMethod,
                                            TestType = testType,
                                            KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                            IutKeys = iutKeys,
                                            IutId = param.IutId,
                                            KeyConfirmationDirection = keyConfInfo.kcDir,
                                            KeyConfirmationRole = keyConfInfo.kcRole,
                                        });
                                    }
                                }
                                else
                                {
                                    groups.Add(new TestGroup()
                                    {
                                        IsSample = param.IsSample,
                                        L = schemeBase.L,
                                        Modulo = modulo,
                                        PublicExponent = exponent,
                                        Scheme = schemeBase.Scheme,
                                        KasMode = schemeBase.KasMode,
                                        KasRole = role,
                                        KdfConfiguration = kdfConfig,
                                        KtsConfiguration = null,
                                        MacConfiguration = null,
                                        TestType = testType,
                                        KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                        IutKeys = iutKeys,
                                        IutId = param.IutId,
                                        KeyConfirmationDirection = keyConfInfo.kcDir,
                                        KeyConfirmationRole = keyConfInfo.kcRole,
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
                                            IsSample = param.IsSample,
                                            L = schemeBase.L,
                                            Modulo = modulo,
                                            PublicExponent = exponent,
                                            Scheme = schemeBase.Scheme,
                                            KasMode = schemeBase.KasMode,
                                            KasRole = role,
                                            KdfConfiguration = null,
                                            KtsConfiguration = ktsConfig,
                                            MacConfiguration = macMethod,
                                            TestType = testType,
                                            KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                            IutKeys = iutKeys,
                                            IutId = param.IutId,
                                            KeyConfirmationDirection = keyConfInfo.kcDir,
                                            KeyConfirmationRole = keyConfInfo.kcRole,
                                        });
                                    }
                                }
                                else
                                {
                                    groups.Add(new TestGroup()
                                    {
                                        IsSample = param.IsSample,
                                        L = schemeBase.L,
                                        Modulo = modulo,
                                        PublicExponent = exponent,
                                        Scheme = schemeBase.Scheme,
                                        KasMode = schemeBase.KasMode,
                                        KasRole = role,
                                        KdfConfiguration = null,
                                        KtsConfiguration = ktsConfig,
                                        MacConfiguration = null,
                                        TestType = testType,
                                        KeyGenerationMethod = keyGenerationMethod.KeyGenerationMethod,
                                        IutKeys = iutKeys,
                                        IutId = param.IutId,
                                        KeyConfirmationDirection = keyConfInfo.kcDir,
                                        KeyConfirmationRole = keyConfInfo.kcRole,
                                    });
                                }
                            }
                        }
                    }
                }                
            }
        }

        private KeyPair[] GetKeys(Parameters parameters)
        {
            // When not sample, the IUT public keys are provided
            if (!parameters.IsSample)
            {
                var keys = new KeyPair[parameters.PublicKeys.Length];
                var count = 0;
                foreach (var key in parameters.PublicKeys)
                {
                    keys[count] = new KeyPair()
                    {
                        PubKey = key
                    };
                    count++;
                }

                return keys;
            }

            return null;
        }

        private (KeyConfirmationDirection kcDir, KeyConfirmationRole kcRole) GetKeyConfirmationInfo(IfcScheme scheme, KeyAgreementRole role)
        {
            switch (scheme)
            {
                case IfcScheme.Kas1_basic:
                case IfcScheme.Kas2_basic:
                case IfcScheme.Kts_oaep_basic:
                    return (KeyConfirmationDirection.None, KeyConfirmationRole.None);
                case IfcScheme.Kas2_bilateral_keyConfirmation:
                    return role == KeyAgreementRole.InitiatorPartyU ? 
                        (KeyConfirmationDirection.Bilateral, KeyConfirmationRole.Provider) : 
                        (KeyConfirmationDirection.Bilateral, KeyConfirmationRole.Recipient);
                case IfcScheme.Kas2_partyU_keyConfirmation:
                    return role == KeyAgreementRole.InitiatorPartyU ?
                        (KeyConfirmationDirection.Unilateral, KeyConfirmationRole.Provider) : 
                        (KeyConfirmationDirection.Unilateral, KeyConfirmationRole.Recipient);
                case IfcScheme.Kas1_partyV_keyConfirmation:
                case IfcScheme.Kas2_partyV_keyConfirmation:
                case IfcScheme.Kts_oaep_partyV_keyConfirmation:
                    return role == KeyAgreementRole.InitiatorPartyU ?
                        (KeyConfirmationDirection.Unilateral, KeyConfirmationRole.Recipient) : 
                        (KeyConfirmationDirection.Unilateral, KeyConfirmationRole.Provider);
                default:
                    throw new ArgumentException($"{nameof(scheme)} {nameof(role)}");
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

        private List<IKdfConfiguration> GetKdfConfigurations(KdfMethods kdfMethods, int l)
        {
            var list = new List<IKdfConfiguration>();

            GetKdfConfiguration(kdfMethods.OneStepKdf, l, list);
            
            return list;
        }

        private void GetKdfConfiguration(OneStepKdf kdfMethodsOneStepKdf, int l, List<IKdfConfiguration> list)
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
                            L = l,
                            FixedInputEncoding = encoding,
                            FixedInputPattern = kdfMethodsOneStepKdf.FixedInputPattern,
                            AuxFunction = auxFunction.AuxFunctionName,
                            SaltMethod = saltMethod,
                            SaltLen = auxFunction.SaltLen
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
                    foreach (var encoding in schemeBaseKtsMethod.Encoding)
                    {
                        list.Add(new KtsConfiguration()
                        {
                            AssociatedDataPattern = schemeBaseKtsMethod.AssociatedDataPattern,
                            KtsHashAlg = hashAlg,
                            Encoding = encoding
                        });                    
                    }
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