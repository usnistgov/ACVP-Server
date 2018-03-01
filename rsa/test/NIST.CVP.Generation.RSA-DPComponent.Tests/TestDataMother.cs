using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_DPComponent.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                var list = new List<AlgoArrayResponseSignature>();

                for(var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var algoArrayResponse = new AlgoArrayResponseSignature
                    {
                        CipherText = new BitString("ABCD"),
                        Key = new KeyPair {PubKey = new PublicKey {E = 1, N = 2}, PrivKey = new PrivateKey{D = 3, P = 4, Q = 5}},
                    };

                    if (testId % 2 == 0)
                    {
                        algoArrayResponse.FailureTest = true;
                    }
                    else
                    {
                        algoArrayResponse.PlainText = new BitString("ABCDEF");
                    }
                    
                    list.Add(algoArrayResponse);
                }
                
                tests.Add(new TestCase {ResultsArray = list});

                testGroups.Add(new TestGroup
                {
                    Modulo = 2048 + groupIdx,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
