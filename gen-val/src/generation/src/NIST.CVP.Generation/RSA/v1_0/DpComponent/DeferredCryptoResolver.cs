﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA.v1_0.DpComponent
{
    public class DeferredCryptoResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, ManyEncryptionResult>
    {
        private readonly IOracle _oracle;

        public DeferredCryptoResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<ManyEncryptionResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
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
                    TestPassed = iutCase.TestPassed,
                    PlainText = iutCase.PlainText
                };

                if (serverResponse.TestPassed)
                {
                    // Should have a PT
                    if (serverResponse.PlainText == null)
                    {
                        serverResponse.TestPassed = false;
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

                        var result = await _oracle.CompleteDeferredRsaDecryptionPrimitiveAsync(param, fullParam);
                        if (result.TestPassed)
                        {
                            serverResponse.CipherText = result.CipherText;
                        }
                        else
                        {
                            serverResponse.TestPassed = false;
                        }
                    }
                }
                
                responses.Add(serverResponse);
            }

            return new ManyEncryptionResult(responses);
        }
    }
}