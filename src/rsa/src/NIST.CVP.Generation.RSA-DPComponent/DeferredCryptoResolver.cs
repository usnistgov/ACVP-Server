using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class DeferredCryptoResolver : IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult>
    {
        private readonly IOracle _oracle;

        public DeferredCryptoResolver(IOracle oracle)
        {
            _oracle = oracle;
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
                        var param = new RsaDecryptionPrimitiveParameters
                        {
                            Modulo = serverTestGroup.Modulo
                        };

                        var fullParam = new RsaDecryptionPrimitiveResult
                        {
                            PlainText = serverResponse.PlainText,
                            Key = serverResponse.Key
                        };

                        var result = _oracle.CompleteDeferredRsaDecryptionPrimitive(param, fullParam);
                        if (result.TestPassed)
                        {
                            serverResponse.CipherText = result.CipherText;
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
