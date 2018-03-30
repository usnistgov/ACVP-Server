using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.AES_OFB
{
    public class AES_OFB_MCT : IAES_OFB_MCT
    {
        private readonly IAES_OFB _iAES_OFB;

        public AES_OFB_MCT(IAES_OFB iAES_OFB)
        {
            _iAES_OFB = iAES_OFB;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
        Key[0] = Key
        IV[0] = IV
        PT[0] = PT
	    For i = 0 to 99
		    Output Key[i]
		    Output IV[i]
		    Output PT[0]
		    For j = 0 to 999
			    If ( j=0 )
				    CT[j] = AES(Key[i], IV[i], PT[j])
				    PT[j+1] = IV[i]
			    Else
				    CT[j] = AES(Key[i], PT[j])
				    PT[j+1] = CT[j-1]
			    Output CT[j]
			    If ( keylen = 128 )
				    Key[i+1] = Key[i] xor CT[j]
			    If ( keylen = 192 )
				    Key[i+1] = Key[i] xor (last 64-bits of CT[j-1] || CT[j])
			    If ( keylen = 256 )
				    Key[i+1] = Key[i] xor (CT[j-1] || CT[j])
		    IV[i+1] = CT[j]
		    PT[0] = CT[j-1]
        */
        #endregion MonteCarloAlgorithm Pseudocode

        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            int i = 0;
            int j = 0;
            try
            {
                for (i = 0; i < 100; i++)
                {
                    AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                    {
                        IV = iv,
                        Key = key,
                        PlainText = plainText
                    };

                    BitString jCipherText = null;
                    BitString previousCipherText = null;
                    BitString copyPreviousCipherText = null;
                    var ivCopiedBytes = iIterationResponse.IV.ToBytes();
                    iv = new BitString(ivCopiedBytes);
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _iAES_OFB.BlockEncrypt(iv, key, plainText);
                        jCipherText = jResult.Result;

                        if (j == 0)
                        {
                            previousCipherText = iIterationResponse.IV;
                        }

                        plainText = previousCipherText;
                        copyPreviousCipherText = previousCipherText;
                        previousCipherText = jCipherText;
                    }

                    iIterationResponse.CipherText = jCipherText;
                    responses.Add(iIterationResponse);

                    if (key.BitLength == 128)
                    {
                        key = key.XOR(previousCipherText);
                    }
                    if (key.BitLength == 192)
                    {
                        var mostSignificant16KeyBitStringXor =
                            key.GetMostSignificantBits(64).XOR( // XOR 64 most significant key bits w/
                                copyPreviousCipherText.Substring(0, 64) // the 64 least significant bits of the previous cipher text
                            );
                        var leastSignificant128KeyBitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(previousCipherText);

                        key = mostSignificant16KeyBitStringXor.ConcatenateBits(leastSignificant128KeyBitStringXor);
                    }
                    if (key.BitLength == 256)
                    {
                        var mostSignificantFirst16BitStringXor = key.GetMostSignificantBits(16 * 8).XOR(copyPreviousCipherText);
                        var leastSignificant16BitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(previousCipherText);
                        key = mostSignificantFirst16BitStringXor.ConcatenateBits(leastSignificant16BitStringXor);
                    }

                    iv = previousCipherText;
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MCTResult<AlgoArrayResponse>(ex.Message);
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            int i = 0;
            int j = 0;
            try
            {
                for (i = 0; i < 100; i++)
                {
                    AlgoArrayResponse iIterationResponse = new AlgoArrayResponse()
                    {
                        IV = iv,
                        Key = key,
                        CipherText = cipherText
                    };

                    BitString jPlainText = null;
                    BitString previousPlainText = null;
                    BitString copyPreviousPlainText = null;
                    var ivCopiedBytes = iIterationResponse.IV.ToBytes();
                    iv = new BitString(ivCopiedBytes);
                    for (j = 0; j < 1000; j++)
                    {
                        var jResult = _iAES_OFB.BlockDecrypt(iv, key, cipherText);
                        jPlainText = jResult.Result;

                        if (j == 0)
                        {
                            previousPlainText = iIterationResponse.IV;
                        }

                        cipherText = previousPlainText;
                        copyPreviousPlainText = previousPlainText;
                        previousPlainText = jPlainText;
                    }

                    iIterationResponse.PlainText = jPlainText;
                    responses.Add(iIterationResponse);

                    if (key.BitLength == 128)
                    {
                        key = key.XOR(previousPlainText);
                    }
                    if (key.BitLength == 192)
                    {
                        var mostSignificant16KeyBitStringXor =
                            key.GetMostSignificantBits(64).XOR( // XOR 64 most significant key bits w/
                                copyPreviousPlainText.Substring(0, 64) // the 64 least significant bits of the previous plain text
                            );
                        var leastSignificant128KeyBitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(previousPlainText);

                        key = mostSignificant16KeyBitStringXor.ConcatenateBits(leastSignificant128KeyBitStringXor);
                    }
                    if (key.BitLength == 256)
                    {
                        var mostSignificantFirst16BitStringXor = key.GetMostSignificantBits(16 * 8).XOR(copyPreviousPlainText);
                        var leastSignificant16BitStringXor = key.GetLeastSignificantBits(16 * 8).XOR(previousPlainText);
                        key = mostSignificantFirst16BitStringXor.ConcatenateBits(leastSignificant16BitStringXor);
                    }

                    iv = previousPlainText;
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MCTResult<AlgoArrayResponse>(ex.Message);
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
