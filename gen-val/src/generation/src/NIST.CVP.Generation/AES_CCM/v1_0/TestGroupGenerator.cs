using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public int[] KeyLens { get; private set; }
        public int[] PtLens { get; private set; }
        public int[] NonceLens { get; private set; }
        public int[] AadLens { get; private set; }
        public int[] TagLens { get; private set; }
        public bool Supports2pow16bytes { get; private set; }

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            PopulateLengths(parameters);

            foreach (InternalTestTypes testType in Enum.GetValues(typeof(InternalTestTypes)))
            {
                CreateGroups(testType, groups);
            }

            return Task.FromResult(groups.AsEnumerable());
        }

        private void PopulateLengths(Parameters parameters)
        {
            KeyLens = parameters.KeyLen;

            // (max PT len value / 8) + 1 gives all possible values 0-32.
            PtLens = parameters.PayloadLen.GetValues(ParameterValidator.VALID_MAX_PT / 8 + 1).ToArray();

            NonceLens = parameters.IvLen.GetValues(ParameterValidator.VALID_NONCE_LENGTHS.Length).ToArray();

            // For AAD, we only want up to a maximum of 32 bytes, so limit the range to 32*8 for bits.
            // Take in a maximum of 33 values (0-32)
            parameters.AadLen.SetMaximumAllowedValue(32 * 8);
            AadLens = parameters.AadLen.GetValues(33).ToArray();

            TagLens = parameters.TagLen.GetValues(ParameterValidator.VALID_TAG_LENGTHS.Length).ToArray();

            Supports2pow16bytes = parameters.SupportsAad2Pow16;
        }

        private void CreateGroups(InternalTestTypes testType, List<TestGroup> groups)
        {
            switch (testType)
            {
                case InternalTestTypes.DecryptionVerification:
                    CreateDecryptionVerificationTestGroups(testType, groups);
                    break;
                case InternalTestTypes.VariableAssociatedData:
                    CreateVariableAssociatedDataTestGroups(testType, groups);
                    break;
                case InternalTestTypes.VariableNonce:
                    CreateVariableNonceTestGroups(testType, groups);
                    break;
                case InternalTestTypes.VariablePayload:
                    CreateVariablePayloadTestGroups(testType, groups);
                    break;
                case InternalTestTypes.VariableTag:
                    CreateVariableTagTestGroups(testType, groups);
                    break;
            }
        }

        /// <summary>
        /// DecryptionVerification creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The minimum and maximum AAD length (not including 2^16 )       
        ///     - The minimum and maximum Payload length
        ///     - Each nonce length
        ///     - Each Tag length
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="parameters"></param>
        /// <param name="groups"></param>
        private void CreateDecryptionVerificationTestGroups(InternalTestTypes testType, List<TestGroup> groups)
        {
            foreach (var keyLen in KeyLens)
            {
                foreach (var aadLen in AadLens
                    .Where(w => w == AadLens.Min() || w == AadLens.Max()))
                {
                    foreach (var ptLen in PtLens
                        .Where(w => w == PtLens.Min() || w == PtLens.Max()))
                    {
                        foreach (var nonceLen in NonceLens)
                        {
                            foreach (var tagLen in TagLens)
                            {
                                TestGroup group = new TestGroup()
                                {
                                    Function = "decrypt",
                                    InternalTestType = testType.ToString(),
                                    KeyLength = keyLen,
                                    AADLength = aadLen,
                                    PayloadLength = ptLen,
                                    IVLength = nonceLen,
                                    TagLength = tagLen,
                                    GroupReusesKeyForTestCases = true
                                };

                                // If the min and max are the same, no need to add the same group
                                if (!groups.Contains(group))
                                {
                                    groups.Add(group);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// VariableAssociatedData creates test cases based on:
        /// 
        ///     - Each key size
        ///     - Each AAD length (including 2^16)
        ///     - The maximum Payload length
        ///     - The maximum nonce length
        ///     - The maximum tag length
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="parameters"></param>
        /// <param name="groups"></param>
        private void CreateVariableAssociatedDataTestGroups(InternalTestTypes testType, List<TestGroup> groups)
        {
            var ptLen = PtLens.Max();
            var nonceLen = NonceLens.Max();
            var tagLen = TagLens.Max();

            foreach (var keyLen in KeyLens)
            {
                foreach (var aadLen in AadLens)
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        InternalTestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PayloadLength = ptLen,
                        IVLength = nonceLen,
                        TagLength = tagLen,
                        GroupReusesKeyForTestCases = true,
                        GroupReusesNonceForTestCases = true
                    };

                    // If the min and max are the same, no need to add the same group
                    if (!groups.Contains(group))
                    {
                        groups.Add(group);
                    }
                }

                if (Supports2pow16bytes)
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        InternalTestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = 65536,
                        PayloadLength = ptLen,
                        IVLength = nonceLen,
                        TagLength = tagLen,
                        GroupReusesKeyForTestCases = true,
                        GroupReusesNonceForTestCases = true
                    };

                    // If the min and max are the same, no need to add the same group
                    if (!groups.Contains(group))
                    {
                        groups.Add(group);
                    }
                }
            }
        }

        /// <summary>
        /// VariableNonce creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The maximum AAD length (excluding 2^16)
        ///     - The maximum Payload length
        ///     - Each nonce length
        ///     - The maximum tag length
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="parameters"></param>
        /// <param name="groups"></param>
        private void CreateVariableNonceTestGroups(InternalTestTypes testType, List<TestGroup> groups)
        {
            var aadLen = AadLens.Max();
            var ptLen = PtLens.Max();
            var tagLen = TagLens.Max();

            foreach (var keyLen in KeyLens)
            {
                foreach (var nonceLen in NonceLens)
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        InternalTestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PayloadLength = ptLen,
                        IVLength = nonceLen,
                        TagLength = tagLen,
                        GroupReusesKeyForTestCases = true
                    };

                    groups.Add(group);
                }
            }
        }

        /// <summary>
        /// VariablePayload creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The maximum AAD length (excluding 2^16)
        ///     - Each Payload length
        ///     - The maximum nonce length
        ///     - The maximum tag length
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="parameters"></param>
        /// <param name="groups"></param>
        private void CreateVariablePayloadTestGroups(InternalTestTypes testType, List<TestGroup> groups)
        {
            var aadLen = AadLens.Max();
            var nonceLen = NonceLens.Max();
            var tagLen = TagLens.Max();

            foreach (var keyLen in KeyLens)
            {
                foreach (var ptLen in PtLens)
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        InternalTestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PayloadLength = ptLen,
                        IVLength = nonceLen,
                        TagLength = tagLen,
                        GroupReusesKeyForTestCases = true,
                        GroupReusesNonceForTestCases = true
                    };

                    // If the min and max are the same, no need to add the same group
                    if (!groups.Contains(group))
                    {
                        groups.Add(group);
                    }
                }
            }
        }

        /// <summary>
        /// VariableTag creates test cases based on:
        /// 
        ///     - Each key size
        ///     - The maximum AAD length (excluding 2^16)
        ///     - The maximum Payload length
        ///     - The maximum nonce length
        ///     - Each tag length
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="parameters"></param>
        /// <param name="groups"></param>
        private void CreateVariableTagTestGroups(InternalTestTypes testType, List<TestGroup> groups)
        {
            var aadLen = AadLens.Max();
            var ptLen = PtLens.Max();
            var nonceLen = NonceLens.Max();

            foreach (var keyLen in KeyLens)
            {
                foreach (var tagLen in TagLens)
                {
                    TestGroup @group = new TestGroup()
                    {
                        Function = "encrypt",
                        InternalTestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PayloadLength = ptLen,
                        IVLength = nonceLen,
                        TagLength = tagLen,
                        GroupReusesKeyForTestCases = true,
                        GroupReusesNonceForTestCases = true
                    };

                    groups.Add(@group);
                }
            }
        }
    }
}