using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
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
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
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

        public async Task<IEnumerable<TTestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            List<TTestGroup> groups = new List<TTestGroup>();

            GenerateGroups(parameters.Scheme, parameters, groups);

            await GenerateDomainParametersAsync(groups);
            
            return groups;
        }

        private void GenerateGroups(Schemes parametersScheme, Parameters param, List<TTestGroup> groups)
        {
            foreach (var scheme in parametersScheme.GetRegisteredSchemes())
            {
                CreateGroupsPerScheme(scheme, param, groups);
            }
        }

        private void CreateGroupsPerScheme(SchemeBase schemeBase, Parameters param, List<TTestGroup> groups)
        {
            if (schemeBase == null)
            {
                return;
            }

            foreach (var testType in TestTypes)
            {
                foreach (var role in schemeBase.KasRole)
                {
                    foreach (var dpGen in param.DomainParameterGenerationMethods)
                    {
                        var kdfMethods = GetKdfConfigurations(testType, role, param.IsSample, schemeBase.KdfMethods,
                            schemeBase.L);

                        var kcScheme = schemeBase.KeyConfirmationMethod != null;
                        foreach (var kdfConfig in kdfMethods)
                        {
                            if (kcScheme)
                            {
                                foreach (var macMethod in GetMacConfigurations(schemeBase.KeyConfirmationMethod
                                    .MacMethods))
                                {
                                    foreach (var kcDirection in schemeBase.KeyConfirmationMethod
                                        .KeyConfirmationDirections)
                                    {
                                        foreach (var kcRole in schemeBase.KeyConfirmationMethod.KeyConfirmationRoles)
                                        {
                                            // Two schemes do not allow key confirmation in a certain direction
                                            if (new[] { KasScheme.EccOnePassDh, KasScheme.FfcDhOneFlow }.Contains(schemeBase.Scheme))
                                            {
                                                if (kcDirection == KeyConfirmationDirection.Bilateral ||
                                                    (role == KeyAgreementRole.InitiatorPartyU &&
                                                     kcRole == KeyConfirmationRole.Provider) ||
                                                    (role == KeyAgreementRole.ResponderPartyV &&
                                                     kcRole == KeyConfirmationRole.Recipient))
                                                {
                                                    continue;
                                                }
                                            }
                                            
                                            groups.Add(new TTestGroup()
                                            {
                                                IsSample = param.IsSample,
                                                L = schemeBase.L,
                                                Scheme = schemeBase.Scheme,
                                                DomainParameterGenerationMode = dpGen,
                                                KasAlgorithm = schemeBase.UnderlyingAlgorithm,
                                                KasMode = KasMode.KdfKc,
                                                KasRole = role,
                                                KdfConfiguration = kdfConfig,
                                                MacConfiguration = macMethod,
                                                TestType = testType,
                                                IutId = param.IutId,
                                                KeyConfirmationDirection = kcDirection,
                                                KeyConfirmationRole = kcRole,
                                            });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                groups.Add(new TTestGroup()
                                {
                                    IsSample = param.IsSample,
                                    L = schemeBase.L,
                                    Scheme = schemeBase.Scheme,
                                    DomainParameterGenerationMode = dpGen,
                                    KasAlgorithm = schemeBase.UnderlyingAlgorithm,
                                    KasMode = KasMode.KdfNoKc,
                                    KasRole = role,
                                    KdfConfiguration = kdfConfig,
                                    MacConfiguration = null,
                                    TestType = testType,
                                    IutId = param.IutId,
                                    KeyConfirmationDirection = KeyConfirmationDirection.None,
                                    KeyConfirmationRole = KeyConfirmationRole.None,
                                });
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
            var registeredMacMethods = schemeBaseMacMethods.GetRegisteredMacMethods().ToList().Shuffle();
            
            var cmacTypes = new[] {KeyAgreementMacType.CmacAes};
            var hmac2Types = new[]
            {
                KeyAgreementMacType.HmacSha2D224,
                KeyAgreementMacType.HmacSha2D256,
                KeyAgreementMacType.HmacSha2D384,
                KeyAgreementMacType.HmacSha2D512,
                KeyAgreementMacType.HmacSha2D512_T224, 
                KeyAgreementMacType.HmacSha2D512_T256,
            };
            var hmac3Types = new[]
            {
                KeyAgreementMacType.HmacSha3D224,
                KeyAgreementMacType.HmacSha3D256,
                KeyAgreementMacType.HmacSha3D384,
                KeyAgreementMacType.HmacSha3D512,
            };
            var kmacTypes = new[] {KeyAgreementMacType.Kmac_128, KeyAgreementMacType.Kmac_256};

            var chosenMacs = new List<MacOptionsBase>();
            
            chosenMacs.AddIfNotNull(registeredMacMethods.FirstOrDefault(w => cmacTypes.Contains(w.MacType)));
            chosenMacs.AddIfNotNull(registeredMacMethods.FirstOrDefault(w => hmac2Types.Contains(w.MacType)));
            chosenMacs.AddIfNotNull(registeredMacMethods.FirstOrDefault(w => hmac3Types.Contains(w.MacType)));
            chosenMacs.AddIfNotNull(registeredMacMethods.FirstOrDefault(w => kmacTypes.Contains(w.MacType)));
            
            foreach (var macMethod in chosenMacs)
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

            // The following KDFs are being removed due to a conversation with the CT group on 2019/10/22.
            //GetKdfConfiguration(testType, role, isSample, kdfMethods.IkeV1Kdf, l, list);
            //GetKdfConfiguration(testType, role, isSample, kdfMethods.IkeV2Kdf, l, list);
            //GetKdfConfiguration(testType, role, isSample, kdfMethods.TlsV10_11Kdf, l, list);
            //GetKdfConfiguration(testType, role, isSample, kdfMethods.TlsV12Kdf, l, list);

            return list;
        }

        private void GetKdfConfiguration(OneStepKdf kdfMethod, int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            foreach (var encoding in kdfMethod.Encoding)
            {
                foreach (var auxFunction in GetKdfMethodAuxFunctions(kdfMethod))
                {
                    int saltLen = 0;
                    switch (auxFunction.AuxFunctionName)
                    {
                        case KasKdfOneStepAuxFunction.HMAC_SHA2_D224:
                        case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224:
                        case KasKdfOneStepAuxFunction.HMAC_SHA3_D224:
                            saltLen = 224;
                            break;
                        case KasKdfOneStepAuxFunction.HMAC_SHA2_D256:
                        case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T256:
                        case KasKdfOneStepAuxFunction.HMAC_SHA3_D256:
                            saltLen = 256;
                            break;
                        case KasKdfOneStepAuxFunction.HMAC_SHA2_D384:
                        case KasKdfOneStepAuxFunction.HMAC_SHA3_D384:
                            saltLen = 384;
                            break;
                        case KasKdfOneStepAuxFunction.HMAC_SHA2_D512:
                        case KasKdfOneStepAuxFunction.HMAC_SHA3_D512:
                            saltLen = 512;
                            break;
                        case KasKdfOneStepAuxFunction.KMAC_128:
                            saltLen = 128;
                            break;
                        case KasKdfOneStepAuxFunction.KMAC_256:
                            saltLen = 256;
                            break;
                    }

                    foreach (var saltMethod in auxFunction.MacSaltMethods.ToList().Shuffle().Take(1))
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
            }
        }

        private AuxFunction[] GetKdfMethodAuxFunctions(OneStepKdf kdfMethod)
        {
            var hmacSha2 = new[]
            {
                KasKdfOneStepAuxFunction.HMAC_SHA2_D224,
                KasKdfOneStepAuxFunction.HMAC_SHA2_D256,
                KasKdfOneStepAuxFunction.HMAC_SHA2_D384,
                KasKdfOneStepAuxFunction.HMAC_SHA2_D512,
                KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224,
                KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224,
            };
            var hmacSha3 = new[]
            {
                KasKdfOneStepAuxFunction.HMAC_SHA3_D224,
                KasKdfOneStepAuxFunction.HMAC_SHA3_D256,
                KasKdfOneStepAuxFunction.HMAC_SHA3_D384,
                KasKdfOneStepAuxFunction.HMAC_SHA3_D512,
            };
            var kmac = new[]
            {
                KasKdfOneStepAuxFunction.KMAC_128,
                KasKdfOneStepAuxFunction.KMAC_256,
            };
            
            var registeredMacMethods = kdfMethod.AuxFunctions.ToList().Shuffle();

            var chosenAuxFunctions = new List<AuxFunction>();
            chosenAuxFunctions.AddIfNotNull(registeredMacMethods.FirstOrDefault(f => hmacSha2.Contains(f.AuxFunctionName)));
            chosenAuxFunctions.AddIfNotNull(registeredMacMethods.FirstOrDefault(f => hmacSha3.Contains(f.AuxFunctionName)));
            chosenAuxFunctions.AddIfNotNull(registeredMacMethods.FirstOrDefault(f => kmac.Contains(f.AuxFunctionName)));
            
            return chosenAuxFunctions.ToArray();
        }

        private void GetKdfConfiguration(TwoStepKdf kdfMethod, int l, List<IKdfConfiguration> list)
        {
            if (kdfMethod == null)
            {
                return;
            }

            List<IKdfConfiguration> tempList = new List<IKdfConfiguration>();

            foreach (var capability in kdfMethod.Capabilities)
            {
                foreach (var encoding in capability.Encoding)
                {
                    foreach (var saltMethod in capability.MacSaltMethods)
                    {
                        foreach (var counterLen in capability.CounterLength)
                        {
                            foreach (var fixedDataOrder in capability.FixedDataOrder)
                            {
                                foreach (var mac in GetCapabilityMacMode(capability))
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
                                        MacMode = mac,
                                        CounterLocation = fixedDataOrder,
                                        CounterLen = counterLen,
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

        private MacModes[] GetCapabilityMacMode(TwoStepCapabilities capability)
        {
            var hmacSha2 = new[]
            {
                MacModes.HMAC_SHA224,
                MacModes.HMAC_SHA256,
                MacModes.HMAC_SHA384,
                MacModes.HMAC_SHA512,
                MacModes.HMAC_SHA_d512t224,
                MacModes.HMAC_SHA_d512t256,
            };
            var hmacSha3 = new[]
            {
                MacModes.HMAC_SHA3_224,
                MacModes.HMAC_SHA3_256,
                MacModes.HMAC_SHA3_384,
                MacModes.HMAC_SHA3_512,
            };
            var cmac = new[]
            {
                MacModes.CMAC_AES128,
                MacModes.CMAC_AES192,
                MacModes.CMAC_AES256,
            };
            
            var registeredMacMethods = capability.MacMode.ToList().Shuffle();

            var chosenMacModes = new List<MacModes>();

            var chosenHmacSha2 = registeredMacMethods.FirstOrDefault(f => hmacSha2.Contains(f));
            if (chosenHmacSha2 != MacModes.None)
            {
                chosenMacModes.Add(chosenHmacSha2);
            }
            
            var chosenHmacSha3 = registeredMacMethods.FirstOrDefault(f => hmacSha3.Contains(f));
            if (chosenHmacSha3 != MacModes.None)
            {
                chosenMacModes.Add(chosenHmacSha3);
            }
            
            var chosenCmac = registeredMacMethods.FirstOrDefault(f => cmac.Contains(f));
            if (chosenCmac != MacModes.None)
            {
                chosenMacModes.Add(chosenCmac);
            }
            
            return chosenMacModes.ToArray();
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
        
        private async Task GenerateDomainParametersAsync(List<TTestGroup> groups)
        {
            var tasks = new Dictionary<TTestGroup, Task<TDomainParameters>>();
            foreach (var group in groups)
            {
                tasks.Add(group, GenerateDomainParametersAsync(group));
            }

            await Task.WhenAll(tasks.Values);

            foreach (var (group, value) in tasks)
            {
                group.DomainParameters = value.Result;
            }
        }

        protected abstract Task<TDomainParameters> GenerateDomainParametersAsync(TTestGroup group);
    }
}