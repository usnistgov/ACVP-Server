using System;
using System.Collections.Generic;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB8
{
    public class AES_CFB8_MCT : IAES_CFB8_MCT
    {
        private readonly IAES_CFB8 _iAES_OFB;

        public AES_CFB8_MCT(IAES_CFB8 iAES_OFB)
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
                    PT[j+1] = ByteJ(IV[i])
                Else
                    CT[j] = AES(Key[i], PT[j])
                    If ( j<16 )
                        PT[j+1] = ByteJ(IV[i])
                    Else
                        PT[j+1] = CT[j-16]
            Output CT[j]
            If ( keylen = 128 )
                Key[i+1] = Key[i] xor (CT[j-15] || CT[j-14] || … || CT[j])
            If ( keylen = 192 )
                Key[i+1] = Key[i] xor (CT[j-23] || CT[j-22] || … || CT[j])
            If ( keylen = 256 )
                Key[i+1] = Key[i] xor (CT[j-31] || CT[j-30] || … || CT[j])
            IV[i+1] = (CT[j-15] || CT[j-14] || … || CT[j])
            PT[0] = CT[j-16]
        */
        #endregion MonteCarloAlgorithm Pseudocode

        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText)
        {
            throw new NotImplementedException();
        }

        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText)
        {
            throw new NotImplementedException();
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
