using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC.Enums
{
    public enum Curve
    {
        P192,
        P224,
        P256,
        P384,
        P521,       // Should be 521 not 512.
        K163,
        K233,
        K283,
        K409,
        K571,
        B163,
        B233,
        B283,
        B409,
        B571
    }

    public enum CurveType
    {
        Prime,
        Binary
    }
}
