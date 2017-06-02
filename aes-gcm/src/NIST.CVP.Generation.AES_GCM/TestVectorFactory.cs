using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = parameters.Algorithm, IsSample = parameters.IsSample};

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var ivLength in parameters.ivLen)
                    {
                        foreach (var ptLength in parameters.PtLen)
                        {
                            foreach (var aadLength in parameters.aadLen)
                            {
                                foreach (var tagLength in parameters.TagLen)
                                {
                                    var testGroup = new TestGroup
                                    {
                                        Function = function,
                                        IVLength = ivLength,
                                        PTLength = ptLength,
                                        KeyLength = keyLength,
                                        AADLength = aadLength,
                                        TagLength = tagLength,
                                        IVGeneration = parameters.ivGen,
                                        IVGenerationMode = parameters.ivGenMode

                                    };
                                    testGroups.Add(testGroup);
                                }
                            }
                        }
                    }
                }
            }
            return testGroups;
        }
    }
}
