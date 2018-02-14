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
                var serverResponse = new AlgoArrayResponse();
                if (iutTestCase.ResultsArray[i].FailureTest)
                {
                    serverResponse.FailureTest = true;
                }
                else
                {
                    var result = _rsa.Encrypt(iutTestCase.ResultsArray[i].PlainText.ToPositiveBigInteger(), iutTestCase.ResultsArray[i].Key.PubKey);
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
