﻿using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1
{
    public class KasValParametersEcc : KasValParametersBase
    {
        public Curve Curve { get; set; }

        public EccScheme EccScheme { get; set; }

        public EccParameterSet EccParameterSet { get; set; }
    }
}