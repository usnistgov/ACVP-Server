using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC.Enums
{
    public enum Curve
    {
        [Description("p-192")]
        P192,
        [Description("p-224")]
        P224,
        [Description("p-256")]
        P256,
        [Description("p-384")]
        P384,
        [Description("p-521")]
        P521,       // Should be 521 not 512.
        [Description("k-163")]
        K163,
        [Description("k-233")]
        K233,
        [Description("k-283")]
        K283,
        [Description("k-409")]
        K409,
        [Description("k-571")]
        K571,
        [Description("b-163")]
        B163,
        [Description("b-233")]
        B233,
        [Description("b-283")]
        B283,
        [Description("b-409")]
        B409,
        [Description("b-571")]
        B571
    }

    public enum CurveType
    {
        Prime,
        Binary
    }

    public enum SecretGenerationMode
    {
        TestingCandidates,
        ExtraRandomBits
    }
}
