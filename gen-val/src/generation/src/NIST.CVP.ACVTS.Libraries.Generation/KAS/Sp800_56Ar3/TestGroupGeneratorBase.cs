using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3
{
    public abstract class TestGroupGeneratorBase<TTestGroup, TTestCase, TDomainParameters, TKeyPair> : ITestGroupGeneratorAsync<Parameters, TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        private static readonly string[] TestTypes =
        {
            "AFT",
            "VAL"
        };

        private ShuffleQueue<KasDpGeneration> _dpGenQueue;
        private int _registeredSchemeCount;
        private int _registeredSchemesWithMacCount;

        public async Task<List<TTestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new HashSet<TTestGroup>();

            _dpGenQueue = new ShuffleQueue<KasDpGeneration>(parameters.DomainParameterGenerationMethods.ToList());

            GenerateGroups(parameters.Scheme, parameters, groups);

            var groupList = groups.ToList();
            await GenerateDomainParametersAsync(groupList);
            await GenerateKeysPerDomainParametersAsync(groupList);

            return groupList;
        }

        private void GenerateGroups(Schemes parametersScheme, Parameters param, HashSet<TTestGroup> groups)
        {
            var schemes = parametersScheme.GetRegisteredSchemes().ToList();
            _registeredSchemeCount = schemes.Count();
            _registeredSchemesWithMacCount = schemes.Count(c => c.KeyConfirmationMethod != null);

            foreach (var scheme in schemes)
            {
                CreateGroupsPerScheme(scheme, param, groups);
            }
        }

        private void CreateGroupsPerScheme(SchemeBase schemeBase, Parameters param, HashSet<TTestGroup> groups)
        {
            if (schemeBase == null)
            {
                return;
            }

            var kdfConfigQueue = GetKdfConfigurations(schemeBase.KdfMethods, schemeBase.L);
            var kasRoles = new ShuffleQueue<KeyAgreementRole>(schemeBase.KasRole.ToList());
            var testTypes = new ShuffleQueue<string>(TestTypes.ToList());

            var kcScheme = schemeBase.KeyConfirmationMethod != null;
            if (kcScheme)
            {
                var macMethodsQueue = GetMacConfigurations(schemeBase.KeyConfirmationMethod.MacMethods);
                var kcDirectionQueue = new ShuffleQueue<KeyConfirmationDirection>(schemeBase.KeyConfirmationMethod.KeyConfirmationDirections.ToList());
                var kcRoleQueue = new ShuffleQueue<KeyConfirmationRole>(schemeBase.KeyConfirmationMethod.KeyConfirmationRoles.ToList());

                var lengthsMax = new[]
                {
                    _dpGenQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount),
                    kasRoles.OriginalListCount,
                    testTypes.OriginalListCount,
                    kdfConfigQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount),
                    _registeredSchemesWithMacCount > 0 ? macMethodsQueue.OriginalListCount.CeilingDivide(_registeredSchemesWithMacCount) : 0,
                    kcDirectionQueue.OriginalListCount,
                    kcRoleQueue.OriginalListCount
                }.Max();

                for (var i = 0; i < lengthsMax; i++)
                {
                    var dpGen = _dpGenQueue.Pop();
                    var kasRole = kasRoles.Pop();
                    var testType = testTypes.Pop();
                    var kdfConfiguration = kdfConfigQueue.Pop();
                    var macMethods = macMethodsQueue.Pop();
                    var kcDirection = kcDirectionQueue.Pop();
                    var kcRole = kcRoleQueue.Pop();

                    if (new[] { KasScheme.EccOnePassDh, KasScheme.FfcDhOneFlow }.Contains(schemeBase.Scheme))
                    {
                        // bilateral key confirmation is not valid for these schemes.
                        if (kcDirection == KeyConfirmationDirection.Bilateral)
                        {
                            continue;
                        }

                        // initiator / provider and responder / recipient are not valid for these schemes
                        if ((kasRole == KeyAgreementRole.InitiatorPartyU &&
                             kcRole == KeyConfirmationRole.Provider) ||
                            (kasRole == KeyAgreementRole.ResponderPartyV &&
                             kcRole == KeyConfirmationRole.Recipient))
                        {
                            kcRole = kcRoleQueue.Pop();
                        }
                    }

                    groups.Add(new TTestGroup()
                    {
                        IsSample = param.IsSample,
                        L = kdfConfiguration.L,
                        Scheme = schemeBase.Scheme,
                        DomainParameterGenerationMode = dpGen,
                        KasAlgorithm = schemeBase.UnderlyingAlgorithm,
                        KasMode = KasMode.KdfKc,
                        KasRole = kasRole,
                        KdfConfiguration = kdfConfiguration,
                        MacConfiguration = macMethods,
                        TestType = testType,
                        IutId = param.IutId,
                        KeyConfirmationDirection = kcDirection,
                        KeyConfirmationRole = kcRole,
                    });
                }
            }
            else
            {
                var lengthsMax = new[]
                {
                    _dpGenQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount),
                    kasRoles.OriginalListCount,
                    testTypes.OriginalListCount,
                    kdfConfigQueue.OriginalListCount,
                }.Max();

                for (var i = 0; i < lengthsMax; i++)
                {
                    var dpGen = _dpGenQueue.Pop();
                    var kasRole = kasRoles.Pop();
                    var testType = testTypes.Pop();
                    var kdfConfiguration = kdfConfigQueue.Pop();

                    groups.Add(new TTestGroup()
                    {
                        IsSample = param.IsSample,
                        L = kdfConfiguration.L,
                        Scheme = schemeBase.Scheme,
                        DomainParameterGenerationMode = dpGen,
                        KasAlgorithm = schemeBase.UnderlyingAlgorithm,
                        KasMode = KasMode.KdfNoKc,
                        KasRole = kasRole,
                        KdfConfiguration = kdfConfiguration,
                        MacConfiguration = null,
                        TestType = testType,
                        IutId = param.IutId,
                        KeyConfirmationDirection = KeyConfirmationDirection.None,
                        KeyConfirmationRole = KeyConfirmationRole.None,
                    });
                }
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

        private readonly Dictionary<ShuffleQueue<IKdfConfiguration>, (List<OneStepConfiguration> oneStep, List<OneStepNoCounterConfiguration> oneStepNoCounter, List<TwoStepConfiguration> twoStep)> _shuffleQueueKdfConfigurations =
            new();
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
                    case KdaOneStepAuxFunction.HMAC_SHA1:
                        saltLen = 160;
                        break;
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
            var auxFunctions = new ShuffleQueue<AuxFunctionNoCounter>(GetKdfMethodAuxFunctions(kdfMethod).ToList());

            var maxCount = new[] { encodings.OriginalListCount, auxFunctions.OriginalListCount }.Max();

            for (var i = 0; i < maxCount; i++)
            {
                var encoding = encodings.Pop();
                var auxFunction = auxFunctions.Pop();

                int saltLen = 0;
                switch (auxFunction.AuxFunctionName)
                {
                    case KdaOneStepAuxFunction.HMAC_SHA1:
                        saltLen = 160;
                        break;
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

        private AuxFunctionNoCounter[] GetKdfMethodAuxFunctions(OneStepNoCounterKdf kdfMethod)
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

        #region unused kdfs
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
        #endregion unused kdfs
        #endregion KDFs

        protected abstract Task GenerateDomainParametersAsync(List<TTestGroup> groups);
        protected abstract Task GenerateKeysPerDomainParametersAsync(List<TTestGroup> groupList);
    }
}
