using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestVectorFactory : ITestVectorFactory
    {
        public TestVectorFactory()
        {
            
        }

        public ITestVectorSet BuildTestVectorSet(IParameters parameters)
        {
            var typedParams = (Parameters)parameters;
            var groups = BuildTestGroups(typedParams);
            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = "AES-GCM", IsSample = typedParams.IsSample};

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
