using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class DeferredCryptoResolver : IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult>
    {
        private readonly IRsa _rsa;

        public DeferredCryptoResolver(IRsa rsa)
        {
            _rsa = rsa;
        }

        public ManyEncryptionResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var responses = new List<AlgoArrayResponseSignature>();

            for (var i = 0; i < serverTestCase.ResultsArray.Count; i++)
            {
                var iutCase = iutTestCase.ResultsArray[i];          // Has key, PT, FailureTest
                var serverCase = serverTestCase.ResultsArray[i];    // Has CT

                var serverResponse = new AlgoArrayResponseSignature
                {
                    CipherText = serverCase.CipherText,
                    Key = iutCase.Key,
                    FailureTest = iutCase.FailureTest,
                    PlainText = iutCase.PlainText
                };

                if (!serverResponse.FailureTest)
                {
                    // Should have a PT
                    if (serverResponse.PlainText == null)
                    {
                        serverResponse.FailureTest = true;
                    }
                    else
                    {
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
                }
                
                responses.Add(serverResponse);
            }

            return new ManyEncryptionResult(responses);
        }
    }
}
