using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
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
                                    PTLength = ptLength,
                                    KeyLength = keyLength,
                                    AADLength = aadLength,
                                    TagLength = tagLength,
                                    IVGeneration = parameters.ivGen,
                                    IVGenerationMode = parameters.ivGenMode,
                                    SaltGen = parameters.SaltGen
                                };
                                testGroups.Add(testGroup);
                            }
                        }
                    }
                }
            }
            return testGroups;
        }
    }
}