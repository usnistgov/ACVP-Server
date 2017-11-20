using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Semantics;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS.FFC.Helpers
{
    /// <summary>
    /// Can be used for creating specific errors (or conditions) within a test case
    /// </summary>
    public static class TestCaseDispositionHelper
    {
        #region MangleKeys
        /// <summary>
        /// Used to mangle a private or public key, based on the <see cref="TestCaseDispositionOption"/>
        /// </summary>
        /// <param name="testCase">The current test case</param>
        /// <param name="dispositionOption">The disposition to </param>
        /// <param name="serverKas"></param>
        /// <param name="iutKas"></param>
        public static void MangleKeys(
            TestCase testCase,
            TestCaseDispositionOption dispositionOption, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas
        )
        {
            var serverKeyExpectations = KeyGenerationRequirementsHelper.GetFfcKeyGenerationOptionsForSchemeAndRole(
                serverKas.Scheme.SchemeParameters.KasDsaAlgoAttributes.Scheme,
                serverKas.Scheme.SchemeParameters.KasMode,
                serverKas.Scheme.SchemeParameters.KeyAgreementRole,
                serverKas.Scheme.SchemeParameters.KeyConfirmationRole,
                serverKas.Scheme.SchemeParameters.KeyConfirmationDirection
            );
            var iutKeyExpectations = KeyGenerationRequirementsHelper.GetFfcKeyGenerationOptionsForSchemeAndRole(
                iutKas.Scheme.SchemeParameters.KasDsaAlgoAttributes.Scheme,
                serverKas.Scheme.SchemeParameters.KasMode,
                iutKas.Scheme.SchemeParameters.KeyAgreementRole,
                iutKas.Scheme.SchemeParameters.KeyConfirmationRole,
                iutKas.Scheme.SchemeParameters.KeyConfirmationDirection
            );

            switch (dispositionOption)
            {
                case TestCaseDispositionOption.FailAssuranceServerStaticPublicKey:
                    MangleServerStaticPublicKey(testCase, serverKas, serverKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey:
                    MangleServerEphemeralPublicKey(testCase, serverKas, serverKeyExpectations.GeneratesEphemeralKeyPair);
                    break;
                case TestCaseDispositionOption.FailAssuranceIutStaticPrivateKey:
                    MangleIutStaticPrivateKey(testCase, iutKas, iutKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case TestCaseDispositionOption.FailAssuranceIutStaticPublicKey:
                    MangleIutStaticPublicKey(testCase, iutKas, iutKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case TestCaseDispositionOption.Success:
                case TestCaseDispositionOption.SuccessLeadingZeroNibbleZ:
                case TestCaseDispositionOption.SuccessLeadingZeroNibbleDkm:
                case TestCaseDispositionOption.FailChangedZ:
                case TestCaseDispositionOption.FailChangedDkm:
                case TestCaseDispositionOption.FailChangedOi:
                case TestCaseDispositionOption.FailChangedMacData:
                case TestCaseDispositionOption.FailChangedTag:
                    // These are not key mangling dispositions, do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dispositionOption), dispositionOption, null);
            }
        }

        private static void MangleServerStaticPublicKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static public key until no longer valid
                while (true)
                {
                    serverKas.Scheme.StaticKeyPair.PublicKeyY += 2;
                    if (!KeyValidationHelper.PerformFfcPublicKeyValidation(
                        serverKas.Scheme.DomainParameters.P, 
                        serverKas.Scheme.DomainParameters.Q, 
                        serverKas.Scheme.StaticKeyPair.PublicKeyY))
                    {
                        break;
                    }
                }
            }
        }

        private static void MangleServerEphemeralPublicKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the ephemeral public key until no longer valid
                while (true)
                {
                    serverKas.Scheme.EphemeralKeyPair.PublicKeyY += 2;
                    if (!KeyValidationHelper.PerformFfcPublicKeyValidation(
                        serverKas.Scheme.DomainParameters.P, 
                        serverKas.Scheme.DomainParameters.Q, 
                        serverKas.Scheme.EphemeralKeyPair.PublicKeyY))
                    {
                        break;
                    }
                }
            }
        }

        private static void MangleIutStaticPrivateKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static private key to make it invalid
                iutKas.Scheme.StaticKeyPair.PrivateKeyX += 2;
            }
        }

        private static void MangleIutStaticPublicKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static public key until no longer valid
                while (true)
                {
                    iutKas.Scheme.StaticKeyPair.PublicKeyY += 2;
                    if (!KeyValidationHelper.PerformFfcPublicKeyValidation(
                        iutKas.Scheme.DomainParameters.P, 
                        iutKas.Scheme.DomainParameters.Q, 
                        iutKas.Scheme.StaticKeyPair.PublicKeyY))
                    {
                        break;
                    }
                }
            }
        }
        #endregion MangleKeys

        public static void SetTestCaseInformationFromKasResults(
            TestGroup group, 
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, 
            KasResult iutResult
        )
        {
            testCase.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateKeyX ?? 0;
            testCase.StaticPublicKeyServer = serverKas.Scheme.StaticKeyPair?.PublicKeyY ?? 0;
            testCase.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateKeyX ?? 0;
            testCase.EphemeralPublicKeyServer = serverKas.Scheme.EphemeralKeyPair?.PublicKeyY ?? 0;
            testCase.EphemeralNonceServer = serverKas.Scheme.EphemeralNonce;

            testCase.StaticPrivateKeyIut = iutKas.Scheme.StaticKeyPair?.PrivateKeyX ?? 0;
            testCase.StaticPublicKeyIut = iutKas.Scheme.StaticKeyPair?.PublicKeyY ?? 0;
            testCase.EphemeralPrivateKeyIut = iutKas.Scheme.EphemeralKeyPair?.PrivateKeyX ?? 0;
            testCase.EphemeralPublicKeyIut = iutKas.Scheme.EphemeralKeyPair?.PublicKeyY ?? 0;
            testCase.EphemeralNonceIut = iutKas.Scheme.EphemeralNonce;

            testCase.Z = iutResult.Z;
            testCase.OtherInfo = iutResult.Oi;
            testCase.OiLen = testCase.OtherInfo?.BitLength ?? 0;
            testCase.Dkm = iutResult.Dkm;
            testCase.MacData = iutResult.MacData;
            if (group.KasMode == KasMode.NoKdfNoKc)
            {
                testCase.HashZ = iutResult.Tag;
            }
            else
            {
                testCase.Tag = iutResult.Tag;
            }
        }

        public static List<TestCaseDispositionOption> PopulateValidityTestCaseOptions(TestGroup testGroup)
        {
            List<TestCaseDispositionOption> validityTestCaseOptions = new List<TestCaseDispositionOption>();
            const int numberOfTestsForValidityGroups = 25;
            const int numberOfScenariosPerType = 2;

            // Can introduce errors/conditions into Oi, Dkm, MacData
            if (testGroup.KasMode != KasMode.NoKdfNoKc)
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedOi, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedDkm, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedMacData, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.SuccessLeadingZeroNibbleDkm, numberOfScenariosPerType);
            }

            // Can always introduce errors/conditions into Z / tag (or hash tag)
            validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedZ, numberOfScenariosPerType);
            validityTestCaseOptions.Add(TestCaseDispositionOption.SuccessLeadingZeroNibbleZ, numberOfScenariosPerType);
            validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedTag, numberOfScenariosPerType);

            // Conditions based on assurances
            if (testGroup.Function.HasFlag(KasAssurance.KeyPairGen) || testGroup.Function.HasFlag(KasAssurance.FullVal))
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceIutStaticPublicKey, numberOfScenariosPerType);
            }

            if (testGroup.Function.HasFlag(KasAssurance.FullVal))
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceServerStaticPublicKey, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, numberOfScenariosPerType);
            }

            if (testGroup.KasMode == KasMode.KdfKc || testGroup.Function.HasFlag(KasAssurance.KeyPairGen) ||
                testGroup.Function.HasFlag(KasAssurance.KeyRegen))
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceIutStaticPrivateKey, numberOfScenariosPerType);
            }

            // Determine number of successful cases to generate
            int numberOfSuccessTests = numberOfTestsForValidityGroups - validityTestCaseOptions.Count;

            validityTestCaseOptions.Add(TestCaseDispositionOption.Success, numberOfSuccessTests);

            // Reorder the test case conditions randomly
            validityTestCaseOptions = validityTestCaseOptions.OrderBy(ob => Guid.NewGuid()).ToList();

            return validityTestCaseOptions;
        }

        public static TestCaseDispositionOption GetTestCaseIntention(List<TestCaseDispositionOption> validityTestCaseOptions)
        {
            var nextDisposition = validityTestCaseOptions[0];
            validityTestCaseOptions.RemoveAt(0);

            return nextDisposition;
        }
    }
}