using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
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
            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = "AES-ECB", IsSample = typedParams.IsSample};

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {

                    foreach (var ptLength in parameters.PtLen)
                    {
                           
                        var testGroup = new TestGroup
                        {
                            Function = function,
                                       
                            PTLength = ptLength,
                            KeyLength = keyLength,
                                      

                        };
                        testGroups.Add(testGroup);
                            
                    }
                    
                }
            }
            return testGroups;
        }
    }
}
