using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const int KeysPerGroup = 100; // can be changed to cause group generation to be much faster for testing purposes

        private static readonly string[] TestTypes =
        {
            "AFT",
            "VAL"
        };
        private static readonly BigInteger DefaultExponent = BigInteger.Zero; // new BigInteger(65537);
        private readonly IOracle _oracle;

        private int _registeredSchemeCount;
        private int _registeredSchemesWithMacCount;
        private ShuffleQueue<IfcKeyGenerationMethod> _keyGenMethodsQueue;
        private ShuffleQueue<int> _moduloQueue;


        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            GenerateGroups(parameters.Scheme, parameters, groups);

            await GenerateKeysForGroups(groups);

            return groups;
        }

        private void GenerateGroups(Schemes parametersScheme, Parameters param, List<TestGroup> groups)
        {
            var schemes = parametersScheme.GetRegisteredSchemes().ToList();
            _registeredSchemeCount = schemes.Count();
            _registeredSchemesWithMacCount = schemes.Count(c => c.MacMethods != null);

            _keyGenMethodsQueue = GetKeyGenMethods(param.KeyGenerationMethods);
            _moduloQueue = GetModulos(param.Modulo);

            foreach (var scheme in schemes)
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

            var testTypeQueue = new ShuffleQueue<string>(TestTypes.ToList());
            var roleQueue = new ShuffleQueue<KeyAgreementRole>(schemeBase.KasRole.ToList());
            var macMethodsQueue = GetMacConfigurations(schemeBase.MacMethods);

            var kdfMethodsQueue = GetKdfConfigurations(schemeBase.KdfMethods, schemeBase.L);
            var ktsConfigurationQueue = GetKtsConfigurations(schemeBase.KtsMethod);

            var isKcScheme = KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(schemeBase.Scheme);
            var isKdfScheme = KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(schemeBase.Scheme);
            var isKtsScheme = KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(schemeBase.Scheme);
            var maxCount = new[]
            {
                roleQueue.OriginalListCount,
                _keyGenMethodsQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount),
                _moduloQueue.OriginalListCount,
                _registeredSchemesWithMacCount > 0 ? macMethodsQueue.OriginalListCount.CeilingDivide(_registeredSchemesWithMacCount) : 0,
                kdfMethodsQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount),
                ktsConfigurationQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount)
            }.Max();

            for (var i = 0; i < maxCount; i++)
            {
                var testType = testTypeQueue.Pop();
                // val tests don't fit with KTS schemes.
                if (isKtsScheme)
                {
                    testType = "AFT";
                }

                var role = roleQueue.Pop();

                var keyConfInfo = GetKeyConfirmationInfo(schemeBase.Scheme, role);
                var keyGenMethod = _keyGenMethodsQueue.Pop();
                var modulo = _moduloQueue.Pop();
                var macMethod = isKcScheme ? macMethodsQueue.Pop() : null;

                var kdfMethod = isKdfScheme ? kdfMethodsQueue.Pop() : null;
                var ktsMethod = isKtsScheme ? ktsConfigurationQueue.Pop() : null;

                var methodsRequiringFixedPublicExponent = new[]
                {
                    IfcKeyGenerationMethod.RsaKpg1_basic,
                    IfcKeyGenerationMethod.RsaKpg1_crt,
                    IfcKeyGenerationMethod.RsaKpg1_primeFactor
                };

                var requiresFixedPublicExponent = methodsRequiringFixedPublicExponent.Contains(keyGenMethod);
                var exponent = requiresFixedPublicExponent
                    ? param.PublicExponent
                    : DefaultExponent;

                if (isKdfScheme)
                {
                    if (isKcScheme)
                    {
                        groups.Add(new TestGroup()
                        {
                            IsSample = param.IsSample,
                            L = kdfMethod.L,
                            Modulo = modulo,
                            PublicExponent = exponent,
                            Scheme = schemeBase.Scheme,
                            KasMode = schemeBase.KasMode,
                            KasRole = role,
                            KdfConfiguration = kdfMethod,
                            KtsConfiguration = null,
                            MacConfiguration = macMethod,
                            TestType = testType,
                            KeyGenerationMethod = keyGenMethod,
                            IutId = param.IutId,
                            KeyConfirmationDirection = keyConfInfo.kcDir,
                            KeyConfirmationRole = keyConfInfo.kcRole,
                        });
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
                            KdfConfiguration = kdfMethod,
                            KtsConfiguration = null,
                            MacConfiguration = null,
                            TestType = testType,
                            KeyGenerationMethod = keyGenMethod,
                            IutId = param.IutId,
                            KeyConfirmationDirection = keyConfInfo.kcDir,
                            KeyConfirmationRole = keyConfInfo.kcRole,
                        });
                    }
                }

                if (isKtsScheme)
                {
                    var hashAlgDigestSizeBytes = ShaAttributes
                        .GetShaAttributes(EnumMapping.GetHashFunctionEnumFromKasHashFunctionEnum(ktsMethod.HashAlg))
                        .outputLen.CeilingDivide(BitString.BITSINBYTE);
                    // This calculation is for the maximum supported "L" given the modulo and hash algorithm
                    var maxGroupL =
                        (modulo.CeilingDivide(BitString.BITSINBYTE) - (2 * hashAlgDigestSizeBytes) - 2) * BitString.BITSINBYTE;

                    var l = System.Math.Min(schemeBase.L, maxGroupL);

                    if (isKcScheme)
                    {
                        groups.Add(new TestGroup()
                        {
                            IsSample = param.IsSample,
                            L = l,
                            Modulo = modulo,
                            PublicExponent = exponent,
                            Scheme = schemeBase.Scheme,
                            KasMode = schemeBase.KasMode,
                            KasRole = role,
                            KdfConfiguration = null,
                            KtsConfiguration = ktsMethod,
                            MacConfiguration = macMethod,
                            TestType = testType,
                            KeyGenerationMethod = keyGenMethod,
                            IutId = param.IutId,
                            KeyConfirmationDirection = keyConfInfo.kcDir,
                            KeyConfirmationRole = keyConfInfo.kcRole,
                        });
                    }
                    else
                    {
                        groups.Add(new TestGroup()
                        {
                            IsSample = param.IsSample,
                            L = l,
                            Modulo = modulo,
                            PublicExponent = exponent,
                            Scheme = schemeBase.Scheme,
                            KasMode = schemeBase.KasMode,
                            KasRole = role,
                            KdfConfiguration = null,
                            KtsConfiguration = ktsMethod,
                            MacConfiguration = null,
                            TestType = testType,
                            KeyGenerationMethod = keyGenMethod,
                            IutId = param.IutId,
                            KeyConfirmationDirection = keyConfInfo.kcDir,
                            KeyConfirmationRole = keyConfInfo.kcRole,
                        });
                    }
                }
            }
        }

        private ShuffleQueue<IfcKeyGenerationMethod> GetKeyGenMethods(IfcKeyGenerationMethod[] keyGenMethods)
        {
            return new ShuffleQueue<IfcKeyGenerationMethod>(keyGenMethods.ToList());
        }

        private ShuffleQueue<int> GetModulos(IEnumerable<int> modulo)
        {
            return new ShuffleQueue<int>(modulo.ToList());
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

        readonly HashSet<ShuffleQueue<MacConfiguration>> _shuffleQueueMacConfigurations = new HashSet<ShuffleQueue<MacConfiguration>>();
        private ShuffleQueue<MacConfiguration> GetMacConfigurations(MacMethods schemeBaseMacMethods)
        {
            if (schemeBaseMacMethods == null)
            {
                return new ShuffleQueue<MacConfiguration>(new List<MacConfiguration>());
            }

            List<MacConfiguration> list = new List<MacConfiguration>();
            var registeredMacMethods = schemeBaseMacMethods.GetRegisteredMacMethods().ToList();

            foreach (var macMethod in registeredMacMethods)
            {
                list.Add(new MacConfiguration()
                {
                    MacType = macMethod.MacType,
                    KeyLen = macMethod.KeyLen,
                    MacLen = macMethod.MacLen
                });
            }

            if (_shuffleQueueMacConfigurations.Count == 0)
            {
                _shuffleQueueMacConfigurations.Add(new ShuffleQueue<MacConfiguration>(list));
            }

            foreach (var item in _shuffleQueueMacConfigurations)
            {
                // if the current mac list is found, return it.
                if (item.OriginalList.Select(s => s.MacType)
                    .All(value => item.OriginalList.Select(s => s.MacType).Contains(value)))
                {
                    return item;
                }
            }

            // otherwise we need to add it as a new item to the dictionary, and use the newly added one.
            var shuffleQueue = new ShuffleQueue<MacConfiguration>(list);
            _shuffleQueueMacConfigurations.Add(shuffleQueue);

            return shuffleQueue;
        }

        #region KDFs
        private readonly Dictionary<ShuffleQueue<IKdfConfiguration>,
            (List<OneStepConfiguration> oneStep, List<OneStepNoCounterConfiguration> oneStepNoCounter, List<TwoStepConfiguration> twoStep)> _shuffleQueueKdfConfigurations = new();
        /// <summary>
        /// Uses a backing Dictionary of ShuffleQueue and OneStep/TwoStep configurations two reuse the shuffle queue for multiple schemes
        /// </summary>
        /// <param name="kdfMethods"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private ShuffleQueue<IKdfConfiguration> GetKdfConfigurations(KdfMethods kdfMethods, int l)
        {
            if (kdfMethods == null)
            {
                return new ShuffleQueue<IKdfConfiguration>(new List<IKdfConfiguration>());
            }

            var list = new List<IKdfConfiguration>();

            var oneStep = GetKdfConfiguration(kdfMethods.OneStepKdf, l);
            var oneStepNoCounter = GetKdfConfiguration(kdfMethods.OneStepNoCounterKdf, l);
            var twoStep = GetKdfConfiguration(kdfMethods.TwoStepKdf, l);

            list.AddRangeIfNotNullOrEmpty(oneStep);
            list.AddRangeIfNotNullOrEmpty(oneStepNoCounter);
            list.AddRangeIfNotNullOrEmpty(twoStep);

            if (_shuffleQueueKdfConfigurations.Count == 0)
            {
                _shuffleQueueKdfConfigurations.Add(new ShuffleQueue<IKdfConfiguration>(list), (oneStep, oneStepNoCounter, twoStep));
            }

            foreach (var kvp in _shuffleQueueKdfConfigurations)
            {
                // if the current oneStep and twoStep list is found within the kvp, return that k.
                if (oneStep.Select(s => s.AuxFunction)
                    .All(value => kvp.Value.oneStep.Select(s => s.AuxFunction).Contains(value)) &&
                    oneStepNoCounter.Select(s => s.AuxFunction)
                        .All(value => kvp.Value.oneStepNoCounter.Select(s => s.AuxFunction).Contains(value)) &&
                    twoStep.Select(s => s.KdfType)
                        .All(value => kvp.Value.twoStep.Select(s => s.KdfType).Contains(value)))
                {
                    return kvp.Key;
                }
            }

            // otherwise we need to add it as a new item to the dictionary, and use the newly added one.
            var shuffleQueue = new ShuffleQueue<IKdfConfiguration>(list);
            _shuffleQueueKdfConfigurations.Add(shuffleQueue, (oneStep, oneStepNoCounter, twoStep));

            return shuffleQueue;
        }

        private List<OneStepConfiguration> GetKdfConfiguration(OneStepKdf kdfMethod, int l)
        {
            if (kdfMethod == null)
            {
                return new List<OneStepConfiguration>();
            }

            var list = new List<OneStepConfiguration>();

            var encodings = new ShuffleQueue<FixedInfoEncoding>(kdfMethod.Encoding.ToList());
            var auxFunctions = new ShuffleQueue<AuxFunction>(GetKdfMethodAuxFunctions(kdfMethod).ToList());

            var maxCount = new[] { encodings.OriginalListCount, auxFunctions.OriginalListCount }.Max();

            for (var i = 0; i < maxCount; i++)
            {
                var encoding = encodings.Pop();
                var auxFunction = auxFunctions.Pop();

                int saltLen = 0;
                switch (auxFunction.AuxFunctionName)
                {
                    case KdaOneStepAuxFunction.HMAC_SHA2_D224:
                    case KdaOneStepAuxFunction.HMAC_SHA2_D512_T224:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D224:
                        saltLen = 224;
                        break;
                    case KdaOneStepAuxFunction.HMAC_SHA2_D256:
                    case KdaOneStepAuxFunction.HMAC_SHA2_D512_T256:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D256:
                        saltLen = 256;
                        break;
                    case KdaOneStepAuxFunction.HMAC_SHA2_D384:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D384:
                        saltLen = 384;
                        break;
                    case KdaOneStepAuxFunction.HMAC_SHA2_D512:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D512:
                        saltLen = 512;
                        break;
                    case KdaOneStepAuxFunction.KMAC_128:
                        saltLen = 128;
                        break;
                    case KdaOneStepAuxFunction.KMAC_256:
                        saltLen = 256;
                        break;
                }

                var saltMethods = new List<MacSaltMethod>();
                if (auxFunction.MacSaltMethods != null && auxFunction.MacSaltMethods.Any())
                {
                    saltMethods = auxFunction.MacSaltMethods.ToList().Shuffle().Take(1).ToList();
                }
                else
                {
                    saltMethods.Add(MacSaltMethod.None);
                }

                foreach (var saltMethod in saltMethods)
                {
                    list.Add(new OneStepConfiguration()
                    {
                        L = l,
                        FixedInfoEncoding = encoding,
                        FixedInfoPattern = kdfMethod.FixedInfoPattern,
                        AuxFunction = auxFunction.AuxFunctionName,
                        SaltMethod = saltMethod,
                        SaltLen = saltLen
                    });
                }
            }

            return list;
        }

        private AuxFunction[] GetKdfMethodAuxFunctions(OneStepKdf kdfMethod)
        {
            return kdfMethod.AuxFunctions;
        }

        private List<OneStepNoCounterConfiguration> GetKdfConfiguration(OneStepNoCounterKdf kdfMethod, int l)
        {
            if (kdfMethod == null)
            {
                return new List<OneStepNoCounterConfiguration>();
            }

            var list = new List<OneStepNoCounterConfiguration>();

            var encodings = new ShuffleQueue<FixedInfoEncoding>(kdfMethod.Encoding.ToList());
            var auxFunctions = new ShuffleQueue<AuxFunctionNoCounter>(GetKdfMethodAuxFunctionsNoCounter(kdfMethod).ToList());

            var maxCount = new[] { encodings.OriginalListCount, auxFunctions.OriginalListCount }.Max();

            for (var i = 0; i < maxCount; i++)
            {
                var encoding = encodings.Pop();
                var auxFunction = auxFunctions.Pop();

                int saltLen = 0;
                switch (auxFunction.AuxFunctionName)
                {
                    case KdaOneStepAuxFunction.HMAC_SHA2_D224:
                    case KdaOneStepAuxFunction.HMAC_SHA2_D512_T224:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D224:
                        saltLen = 224;
                        break;
                    case KdaOneStepAuxFunction.HMAC_SHA2_D256:
                    case KdaOneStepAuxFunction.HMAC_SHA2_D512_T256:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D256:
                        saltLen = 256;
                        break;
                    case KdaOneStepAuxFunction.HMAC_SHA2_D384:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D384:
                        saltLen = 384;
                        break;
                    case KdaOneStepAuxFunction.HMAC_SHA2_D512:
                    case KdaOneStepAuxFunction.HMAC_SHA3_D512:
                        saltLen = 512;
                        break;
                    case KdaOneStepAuxFunction.KMAC_128:
                        saltLen = 128;
                        break;
                    case KdaOneStepAuxFunction.KMAC_256:
                        saltLen = 256;
                        break;
                }

                var saltMethods = new List<MacSaltMethod>();
                if (auxFunction.MacSaltMethods != null && auxFunction.MacSaltMethods.Any())
                {
                    saltMethods = auxFunction.MacSaltMethods.ToList().Shuffle().Take(1).ToList();
                }
                else
                {
                    saltMethods.Add(MacSaltMethod.None);
                }

                foreach (var saltMethod in saltMethods)
                {
                    list.Add(new OneStepNoCounterConfiguration()
                    {
                        L = auxFunction.L,
                        FixedInfoEncoding = encoding,
                        FixedInfoPattern = kdfMethod.FixedInfoPattern,
                        AuxFunction = auxFunction.AuxFunctionName,
                        SaltMethod = saltMethod,
                        SaltLen = saltLen
                    });
                }
            }

            return list;
        }

        private AuxFunctionNoCounter[] GetKdfMethodAuxFunctionsNoCounter(OneStepNoCounterKdf kdfMethod)
        {
            return kdfMethod.AuxFunctions;
        }

        private List<TwoStepConfiguration> GetKdfConfiguration(TwoStepKdf kdfMethod, int l)
        {
            if (kdfMethod == null)
            {
                return new List<TwoStepConfiguration>();
            }

            List<TwoStepConfiguration> tempList = new List<TwoStepConfiguration>();

            foreach (var capability in kdfMethod.Capabilities)
            {
                var encodings = new ShuffleQueue<FixedInfoEncoding>(capability.Encoding.ToList());
                var saltMethods = new ShuffleQueue<MacSaltMethod>(capability.MacSaltMethods.ToList());
                var counterLengths = new ShuffleQueue<int>(capability.CounterLength.ToList());
                var counterLocations = new ShuffleQueue<CounterLocations>(capability.FixedDataOrder.ToList());
                var macModes = new ShuffleQueue<MacModes>(GetCapabilityMacMode(capability).ToList());

                var maxLength = new[]
                {
                    encodings.OriginalListCount,
                    saltMethods.OriginalListCount,
                    counterLengths.OriginalListCount,
                    counterLocations.OriginalListCount,
                    macModes.OriginalListCount
                }.Max();

                for (var i = 0; i < maxLength; i++)
                {
                    var encoding = encodings.Pop();
                    var saltMethod = saltMethods.Pop();
                    var counterLength = counterLengths.Pop();
                    var counterLocation = counterLocations.Pop();
                    var macMode = macModes.Pop();

                    // If counter length is 0, only do the 'none', otherwise, skip the 'none'
                    if (counterLength == 0)
                    {
                        if (counterLocation != CounterLocations.None)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (counterLocation == CounterLocations.None)
                        {
                            continue;
                        }
                    }

                    // Cannot generate groups when the counter is in the "middle".
                    if (counterLocation == CounterLocations.MiddleFixedData)
                    {
                        continue;
                    }

                    var saltLen = 0;
                    var ivLen = 0;
                    switch (macMode)
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
                        case MacModes.HMAC_SHA3_224:
                            saltLen = 224;
                            ivLen = 224;
                            break;
                        case MacModes.HMAC_SHA256:
                        case MacModes.HMAC_SHA3_256:
                            saltLen = 256;
                            ivLen = 256;
                            break;
                        case MacModes.HMAC_SHA384:
                        case MacModes.HMAC_SHA3_384:
                            saltLen = 384;
                            ivLen = 384;
                            break;
                        case MacModes.HMAC_SHA512:
                        case MacModes.HMAC_SHA3_512:
                            saltLen = 512;
                            ivLen = 512;
                            break;
                    }

                    if (capability.KdfMode != KdfModes.Feedback ||
                        (capability.KdfMode == KdfModes.Feedback && (capability.RequiresEmptyIv)))
                    {
                        ivLen = 0;
                    }

                    tempList.Add(new TwoStepConfiguration()
                    {
                        L = l,
                        SaltLen = saltLen,
                        FixedInfoEncoding = encoding,
                        FixedInfoPattern = capability.FixedInfoPattern,
                        SaltMethod = saltMethod,
                        KdfMode = capability.KdfMode,
                        MacMode = macMode,
                        CounterLocation = counterLocation,
                        CounterLen = counterLength,
                        IvLen = ivLen
                    });

                    // Only Feedback has an IV, so add additional group of 0 len iv when feedback group supports it,
                    // and it's not required (as that would have been covered in the above add.
                    if (capability.SupportsEmptyIv && !capability.RequiresEmptyIv && capability.KdfMode == KdfModes.Feedback)
                    {
                        tempList.Add(new TwoStepConfiguration()
                        {
                            L = l,
                            SaltLen = saltLen,
                            FixedInfoEncoding = encoding,
                            FixedInfoPattern = capability.FixedInfoPattern,
                            SaltMethod = saltMethod,
                            KdfMode = capability.KdfMode,
                            MacMode = macMode,
                            CounterLocation = counterLocation,
                            CounterLen = counterLength,
                            IvLen = 0
                        });
                    }
                }
            }

            // No need to fully test each enumeration of this KDF, as it is tested separately, take max of 5 groups randomly.
            return tempList;
        }

        private MacModes[] GetCapabilityMacMode(TwoStepCapabilities capability)
        {
            return capability.MacMode;
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

        readonly HashSet<ShuffleQueue<KtsConfiguration>> _shuffleQueueKtsConfigurations = new HashSet<ShuffleQueue<KtsConfiguration>>();
        private ShuffleQueue<KtsConfiguration> GetKtsConfigurations(KtsMethod schemeBaseKtsMethod)
        {
            if (schemeBaseKtsMethod == null)
            {
                return new ShuffleQueue<KtsConfiguration>(new List<KtsConfiguration>());
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
                            HashAlg = hashAlg,
                            Encoding = encoding
                        });
                    }
                }

                if (schemeBaseKtsMethod.SupportsNullAssociatedData)
                {
                    list.Add(new KtsConfiguration()
                    {
                        AssociatedDataPattern = string.Empty,
                        HashAlg = hashAlg
                    });
                }
            }

            if (_shuffleQueueMacConfigurations.Count == 0)
            {
                _shuffleQueueKtsConfigurations.Add(new ShuffleQueue<KtsConfiguration>(list));
            }

            foreach (var item in _shuffleQueueKtsConfigurations)
            {
                // if the current mac list is found, return it.
                if (item.OriginalList.Select(s => s.HashAlg)
                    .All(value => item.OriginalList.Select(s => s.HashAlg).Contains(value)))
                {
                    return item;
                }
            }

            // otherwise we need to add it as a new item to the dictionary, and use the newly added one.
            var shuffleQueue = new ShuffleQueue<KtsConfiguration>(list);
            _shuffleQueueKtsConfigurations.Add(shuffleQueue);

            return shuffleQueue;
        }

        private async Task GenerateKeysForGroups(List<TestGroup> groups)
        {
            var moduloExponentPairs = groups.Select(s => new
            {
                s.Modulo,
                s.PublicExponent,
                s.PrivateKeyMode

            }).Distinct();
            var tasks = new Dictionary<(int modulo, BigInteger publicExponent, PrivateKeyModes privateKeyMode), List<Task<KeyPair>>>();

            foreach (var pair in moduloExponentPairs)
            {
                var list = new List<Task<KeyPair>>();
                for (var i = 0; i < KeysPerGroup; i++)
                {
                    list.Add(GetKey(pair.Modulo, pair.PublicExponent, pair.PrivateKeyMode));
                }

                tasks.Add((pair.Modulo, pair.PublicExponent, pair.PrivateKeyMode), list);
            }

            await Task.WhenAll(tasks.SelectMany(s => s.Value));

            var shuffleQueues = new Dictionary<(int modulo, BigInteger publicExponent, PrivateKeyModes privateKeyMode), ShuffleQueue<KeyPair>>();
            foreach (var kvp in tasks)
            {
                List<KeyPair> keys = new List<KeyPair>();
                foreach (var task in kvp.Value)
                {
                    keys.Add(await task);
                }
                shuffleQueues.Add(kvp.Key, new ShuffleQueue<KeyPair>(keys));
            }

            foreach (var group in groups)
            {
                group.ShuffleKeys = shuffleQueues
                    .First(f => f.Key.modulo == group.Modulo &&
                                f.Key.publicExponent == group.PublicExponent &&
                                f.Key.privateKeyMode == group.PrivateKeyMode).Value;
            }
        }

        private async Task<KeyPair> GetKey(int modulo, BigInteger publicExponent, PrivateKeyModes privateKeyMode)
        {
            var task = _oracle.GetRsaKeyAsync(new RsaKeyParameters()
            {
                Modulus = modulo,
                Standard = Fips186Standard.Fips186_5,
                KeyFormat = privateKeyMode,
                KeyMode = PrimeGenModes.RandomProbablePrimes,
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                PublicExponentMode = publicExponent == 0 ? PublicExponentModes.Random : PublicExponentModes.Fixed,
                PublicExponent = publicExponent == 0 ? null : new BitString(publicExponent)
            });

            var result = await task;
            return result.Key;
        }
    }
}
