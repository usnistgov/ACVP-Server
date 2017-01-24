using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_OFB
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

        public MCTResult MCTEncrypt(BitString iv, BitString key, BitString plainText)
        {
            throw new NotImplementedException();
        }

        public MCTResult MCTDecrypt(BitString iv, BitString key, BitString cipherText)
        {
            throw new NotImplementedException();
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
