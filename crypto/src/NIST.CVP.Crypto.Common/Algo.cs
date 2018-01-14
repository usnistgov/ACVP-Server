using System;
using System.ComponentModel;

namespace NIST.CVP.Crypto.Common
{
    public enum Algo
    {
        [Description("TDES-CFB1")]
        TDES_CFB1,
        [Description("TDES-CFB8")]
        TDES_CFB8,
        [Description("TDES-CFB64")]
        TDES_CFB64,
        [Description("AES-CFB1")]
        AES_CFB1,
        [Description("AES-CFB8")]
        AES_CFB8,
        [Description("AES-CFB128")]
        AES_CFB128,
        [Description("TDES-CFBP1")]
        TDES_CFBP1,
        [Description("TDES-CFBP8")]
        TDES_CFBP8,
        [Description("TDES-CFBP64")]
        TDES_CFBP64

    }
}