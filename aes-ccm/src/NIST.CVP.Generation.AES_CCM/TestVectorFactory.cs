using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = new List<ITestGroup>();

            foreach (TestTypes testType in Enum.GetValues(typeof(TestTypes)))
            {
                CreateGroups(testType, parameters, groups);
            }

            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = "AES-CCM", IsSample = parameters.IsSample};

            return testVector;
        }

        private void CreateGroups(TestTypes testType, Parameters parameters, List<ITestGroup> groups)
        {
            switch (testType)
            {
                case TestTypes.DecryptionVerification:
                    CreateDecryptionVerificationTestGroups(testType, parameters, groups);
                    break;
                case TestTypes.VariableAssociatedData:
                    CreateVariableAssocatedDataTestGroups(testType, parameters, groups);
                    break;
                case TestTypes.VariableNonce:
                    CreateVariableNonceTestGroups(testType, parameters, groups);
                    break;
                case TestTypes.VariablePayload:
                    CreateVariablePayloadTestGroups(testType, parameters, groups);
                    break;
                case TestTypes.VariableTag:
                    CreateVariableTagTestGroups(testType, parameters, groups);
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
        private void CreateDecryptionVerificationTestGroups(TestTypes testType, Parameters parameters, List<ITestGroup> groups)
        {
            foreach (var keyLen in parameters.KeyLen)
            {
                foreach (var aadLen in parameters.AadLen
                    .GetMinMaxAsEnumerable()
                    .Where(w => w == parameters.AadLen.Min || w == parameters.AadLen.Max))
                {
                    foreach (var ptLen in parameters.PtLen
                        .GetMinMaxAsEnumerable()
                        .Where(w => w == parameters.PtLen.Min || w == parameters.PtLen.Max))
                    {
                        foreach (var nonceLen in parameters.Nonce)
                        {
                            foreach (var tagLen in parameters.TagLen)
                            {
                                TestGroup group = new TestGroup()
                                {
                                    Function = "decrypt",
                                    TestType = testType.ToString(),
                                    KeyLength = keyLen,
                                    AADLength = aadLen,
                                    PTLength = ptLen,
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
        /// VariableAssocatedData creates test cases based on:
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
        private void CreateVariableAssocatedDataTestGroups(TestTypes testType, Parameters parameters, List<ITestGroup> groups)
        {
            var ptLen = parameters.PtLen.Max;
            var nonceLen = parameters.Nonce.Max();
            var tagLen = parameters.TagLen.Max();

            foreach (var keyLen in parameters.KeyLen)
            {
                foreach (var aadLen in parameters.AadLen.GetValues())
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        TestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PTLength = ptLen,
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

                if (parameters.SupportsAad2Pow16)
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        TestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = 65536,
                        PTLength = ptLen,
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
        private void CreateVariableNonceTestGroups(TestTypes testType, Parameters parameters, List<ITestGroup> groups)
        {
            var aadLen = parameters.AadLen.Max;
            var ptLen = parameters.PtLen.Max;
            var tagLen = parameters.TagLen.Max();

            foreach (var keyLen in parameters.KeyLen)
            {
                foreach (var nonceLen in parameters.Nonce)
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        TestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PTLength = ptLen,
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
        private void CreateVariablePayloadTestGroups(TestTypes testType, Parameters parameters, List<ITestGroup> groups)
        {
            var aadLen = parameters.AadLen.Max;
            var nonceLen = parameters.Nonce.Max();
            var tagLen = parameters.TagLen.Max();

            foreach (var keyLen in parameters.KeyLen)
            {
                foreach (var ptLen in parameters.PtLen.GetValues())
                {
                    TestGroup group = new TestGroup()
                    {
                        Function = "encrypt",
                        TestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PTLength = ptLen,
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
        private void CreateVariableTagTestGroups(TestTypes testType, Parameters parameters, List<ITestGroup> groups)
        {
            var aadLen = parameters.AadLen.Max;
            var ptLen = parameters.PtLen.Max;
            var nonceLen = parameters.Nonce.Max();
            
            foreach (var keyLen in parameters.KeyLen)
            {
                foreach (var tagLen in parameters.TagLen)
                {
                    TestGroup @group = new TestGroup()
                    {
                        Function = "encrypt",
                        TestType = testType.ToString(),
                        KeyLength = keyLen,
                        AADLength = aadLen,
                        PTLength = ptLen,
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
