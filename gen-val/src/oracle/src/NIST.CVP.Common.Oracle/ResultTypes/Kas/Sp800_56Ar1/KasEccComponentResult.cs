﻿using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1
{
    public class KasEccComponentResult
    {
        
        public BigInteger PrivateKeyServer { get; set; }

        public BigInteger PublicKeyServerX { get; set; }

        public BigInteger PublicKeyServerY { get; set; }


        #region Sample Only Properties
        public BigInteger PrivateKeyIut { get; set; }

        public BigInteger PublicKeyIutX { get; set; }
                                   
        public BigInteger PublicKeyIutY { get; set; }

        public BitString Z { get; set; }
        #endregion Sample Only Properties
    }
}