using System;
using System.Collections.Generic;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class AES_CFB1_MCT : IAES_CFB1_MCT
    {
        private readonly IAES_CFB1 _algo;

        public AES_CFB1_MCT(IAES_CFB1 algo)
        {
            _algo = algo;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
        
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
