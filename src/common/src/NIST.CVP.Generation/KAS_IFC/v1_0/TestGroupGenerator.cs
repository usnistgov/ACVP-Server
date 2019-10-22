using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private static readonly string[] TestTypes =
        {
            "AFT",
            "VAL"
        };
        private static readonly BigInteger DefaultExponent = BigInteger.Zero; // new BigInteger(65537);

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

            foreach (var testType in TestTypes)
            {
                var iutKeys = testType.Equals("AFT", StringComparison.OrdinalIgnoreCase) ? GetKeys(param) : null;

                foreach (var role in schemeBase.KasRole)
                {
                    var keyConfInfo = GetKeyConfirmationInfo(schemeBase.Scheme, role);
                    var kdfMethods = GetKdfConfigurations(testType, role, param.IsSample, schemeBase.KdfMethods, schemeBase.L);

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

        private IutKeys[] GetKeys(Parameters parameters)
        {
            // When not sample, the IUT public keys are provided
            if (!parameters.IsSample)
            {
                return parameters.IutKeys;
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

        #region KDFs
        private List<IKdfConfiguration> GetKdfConfigurations(string testType, KeyAgreementRole role, bool isSample, KdfMethods kdfMethods, int l)
        {
            if (kdfMethods == null)
            {
                return new List<IKdfConfiguration>();
            }

            var list = new List<IKdfConfiguration>();

            GetKdfConfiguration(kdfMethods.OneStepKdf, l, list);
            GetKdfConfiguration(kdfMethods.TwoStepKdf, l, list);
            GetKdfConfiguration(testType, role, isSample, kdfMethods.IkeV1Kdf, l, list);
            GetKdfConfiguration(testType, role, isSample, kdfMethods.IkeV2Kdf, l, list);
            GetKdfConfiguration(testType, role, isSample, kdfMethods.TlsV10_11Kdf, l, list);
            GetKdfConfiguration(testType, role, isSample, kdfMethods.TlsV12Kdf, l, list);

            return list;
        }

        private void GetKdfConfiguration(OneStepKdf kdfMethod, int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            // Since this KDF is specific to KAS, test each enumeration
            foreach (var encoding in kdfMethod.Encoding)
            {
                foreach (var auxFunction in kdfMethod.AuxFunctions)
                {
                    foreach (var saltMethod in auxFunction.MacSaltMethods)
                    {
                        list.Add(new OneStepConfiguration()
                        {
                            L = l,
                            FixedInputEncoding = encoding,
                            FixedInputPattern = kdfMethod.FixedInputPattern,
                            AuxFunction = auxFunction.AuxFunctionName,
                            SaltMethod = saltMethod,
                            SaltLen = auxFunction.SaltLen
                        });
                    }
                }
            }
        }

        private void GetKdfConfiguration(TwoStepKdf kdfMethod, int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            List<IKdfConfiguration> tempList = new List<IKdfConfiguration>();

            foreach (var encoding in kdfMethod.Encoding)
            {
                foreach (var saltMethod in kdfMethod.MacSaltMethods)
                {
                    foreach (var capability in kdfMethod.Capabilities)
                    {
                        foreach (var counterLen in capability.CounterLength)
                        {
                            foreach (var fixedDataOrder in capability.FixedDataOrder)
                            {
                                foreach (var mac in capability.MacMode)
                                {
                                    // If counter length is 0, only do the 'none', otherwise, skip the 'none'
                                    if (counterLen == 0)
                                    {
                                        if (fixedDataOrder != CounterLocations.None)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (fixedDataOrder == CounterLocations.None)
                                        {
                                            continue;
                                        }
                                    }

                                    // Cannot generate groups when the counter is in the "middle".
                                    if (fixedDataOrder == CounterLocations.MiddleFixedData)
                                    {
                                        continue;
                                    }

                                    var saltLen = 0;
                                    var ivLen = 0;
                                    switch (mac)
                                    {
                                        case MacModes.CMAC_AES128:
                                            saltLen = 128;
                                            ivLen = 128;
                                            break;
                                        case MacModes.CMAC_AES192:
                                            saltLen = 192;
                                            ivLen = 128;
                                            break;
                                        case MacModes.CMAC_AES256:
                                            saltLen = 256;
                                            ivLen = 128;
                                            break;
                                        case MacModes.CMAC_TDES:
                                            continue;
                                        case MacModes.HMAC_SHA1:
                                            saltLen = 160;
                                            ivLen = 160;
                                            break;
                                        case MacModes.HMAC_SHA224:
                                            saltLen = 224;
                                            ivLen = 224;
                                            break;
                                        case MacModes.HMAC_SHA256:
                                            saltLen = 256;
                                            ivLen = 256;
                                            break;
                                        case MacModes.HMAC_SHA384:
                                            saltLen = 384;
                                            ivLen = 384;
                                            break;
                                        case MacModes.HMAC_SHA512:
                                            saltLen = 512;
                                            ivLen = 512;
                                            break;
                                    }

                                    if (capability.KdfMode != KdfModes.Feedback)
                                    {
                                        ivLen = 0;
                                    }

                                    tempList.Add(new TwoStepConfiguration()
                                    {
                                        L = l,
                                        SaltLen = saltLen,
                                        FixedInputEncoding = encoding,
                                        FixedInputPattern = kdfMethod.FixedInputPattern,
                                        SaltMethod = saltMethod,
                                        KdfMode = capability.KdfMode,
                                        MacMode = mac,
                                        CounterLocation = fixedDataOrder,
                                        CounterLen = counterLen,
                                        IvLen = ivLen
                                    });

                                    // Only Feedback has an IV, so add additional group of 0 len iv when feedback group supports it.
                                    if (capability.SupportsEmptyIv && capability.KdfMode == KdfModes.Feedback)
                                    {
                                        tempList.Add(new TwoStepConfiguration()
                                        {
                                            L = l,
                                            SaltLen = saltLen,
                                            FixedInputEncoding = encoding,
                                            FixedInputPattern = kdfMethod.FixedInputPattern,
                                            SaltMethod = saltMethod,
                                            KdfMode = capability.KdfMode,
                                            MacMode = mac,
                                            CounterLocation = fixedDataOrder,
                                            CounterLen = counterLen,
                                            IvLen = 0
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // No need to fully test each enumeration of this KDF, as it is tested separately, take max of 5 groups randomly.
            list.AddRangeIfNotNullOrEmpty(tempList.Shuffle().Take(5));
        }

        private void GetKdfConfiguration(string testType, KeyAgreementRole role, bool isSample, IkeV1Kdf kdfMethod, int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            List<IKdfConfiguration> tempList = new List<IKdfConfiguration>();

            foreach (var hashAlg in kdfMethod.HashFunctions)
            {
                var hashFunction = ShaAttributes.GetHashFunctionFromEnum(hashAlg);
                // TODO remove multiply by 3 if concatenation is not used.
                var outputLen = hashFunction.OutputLen * 3;

                if (l <= outputLen)
                {
                    tempList.Add(new IkeV1Configuration()
                    {
                        L = l,
                        HashFunction = hashAlg,
                        ServerGenerateInitiatorAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, true),
                        ServerGenerateResponderAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, false),
                    });
                }
            }

            // No need to fully test each enumeration of this KDF, as it is tested separately, take max of 5 groups randomly.
            list.AddRangeIfNotNullOrEmpty(tempList.Shuffle().Take(5));
        }

        private void GetKdfConfiguration(string testType, KeyAgreementRole role, bool isSample, IkeV2Kdf kdfMethod, int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            List<IKdfConfiguration> tempList = new List<IKdfConfiguration>();

            foreach (var hashAlg in kdfMethod.HashFunctions)
            {
                tempList.Add(new IkeV2Configuration()
                {
                    L = l,
                    HashFunction = hashAlg,
                    ServerGenerateInitiatorAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, true),
                    ServerGenerateResponderAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, false),
                });
            }

            // No need to fully test each enumeration of this KDF, as it is tested separately, take max of 5 groups randomly.
            list.AddRangeIfNotNullOrEmpty(tempList.Shuffle().Take(5));
        }

        private void GetKdfConfiguration(string testType, KeyAgreementRole role, bool isSample, TlsV10_11Kdf kdfMethod,
            int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            list.Add(new Tls10_11Configuration()
            {
                L = l,
                ServerGenerateInitiatorAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, true),
                ServerGenerateResponderAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, false),
            });
        }

        private void GetKdfConfiguration(string testType, KeyAgreementRole role, bool isSample, TlsV12Kdf kdfMethod,
            int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            foreach (var hashFunction in kdfMethod.HashFunctions)
            {
                list.Add(new Tls12Configuration()
                {
                    L = l,
                    HashFunction = hashFunction,
                    ServerGenerateInitiatorAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, true),
                    ServerGenerateResponderAdditionalNonce = ShouldCreateAdditionalNonce(testType, role, isSample, false),
                });
            }
        }

        /// <summary>
        /// Determines if an additional nonce should be created based on the test type, iutRole, sample flag,
        /// and whether or not we're checking against the initiator or responder nonce generation.
        /// </summary>
        /// <param name="testType">The test type for the group.</param>
        /// <param name="iutRole">The IUT role for the group.</param>
        /// <param name="isSample">Is this a sample vector set?</param>
        /// <param name="checkingInitiatorNonceRequirement">Are we checking if the initiator should have an additional nonce generator, or the responder?</param>
        /// <returns></returns>
        private bool ShouldCreateAdditionalNonce(string testType, KeyAgreementRole iutRole, bool isSample, bool checkingInitiatorNonceRequirement)
        {
            // We need to create all additional nonces when sample or val test type, where everything is generated up front.
            if (isSample || testType.Equals("VAL", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            switch (iutRole)
            {
                // we don't want to generate the initiator nonce if the iut is the initiator.
                case KeyAgreementRole.InitiatorPartyU:
                    return !checkingInitiatorNonceRequirement;
                // we want to generate the initiator nonce if the iut is the responder.
                case KeyAgreementRole.ResponderPartyV:
                    return checkingInitiatorNonceRequirement;
                default:
                    throw new ArgumentException($"Invalid {nameof(iutRole)}");
            }
        }
        #endregion KDFs

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