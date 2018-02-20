using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class DeferredCryptoResolver : IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult>
    {
        private readonly IRsa _rsa;

        public DeferredCryptoResolver(IRsa rsa)
        {
            _rsa = rsa;
        }

        public ManyEncryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var responses = new List<AlgoArrayResponse>();

            for (var i = 0; i < serverTestCase.ResultsArray.Count; i++)
            {
                var iutCase = iutTestCase.ResultsArray[i];          // Has key, PT, FailureTest
                var serverCase = serverTestCase.ResultsArray[i];    // Has CT

                var serverResponse = new AlgoArrayResponse
                {
                    CipherText = serverCase.CipherText,
                    Key = iutCase.Key,
                    FailureTest = iutCase.FailureTest,
                    PlainText = iutCase.PlainText
                };

                if (!serverResponse.FailureTest)
                {
                    // Should have a PT
                    var result = _rsa.Encrypt(serverResponse.PlainText.ToPositiveBigInteger(), serverResponse.Key.PubKey);
                    if (result.Success)
                    {
                        serverResponse.CipherText = new BitString(result.CipherText);
                    }
                    else
                    {
                        serverResponse.FailureTest = true;
                    }
                }
                
                responses.Add(serverResponse);
            }

            return new ManyEncryptionResult(responses);
        }
    }
}
