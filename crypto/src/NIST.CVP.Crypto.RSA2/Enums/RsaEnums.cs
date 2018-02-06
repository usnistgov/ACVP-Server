using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Enums
{
    public enum PrimeGenModes
    {
        [Description("b.3.2")]
        B32,

        [Description("b.3.3")]
        B33,

        [Description("b.3.4")]
        B34,

        [Description("b.3.5")]
        B35,

        [Description("b.3.6")]
        B36
    }

    public enum PrimeTestModes
    {
        [Description("tblc2")]
        C2,

        [Description("tblc3")]
        C3
    }

    public enum SignatureSchemes
    {
        [Description("ansx9.31")]
        Ansx931,

        [Description("pkcs1v15")]
        Pkcs1v15,

        [Description("pss")]
        Pss
    }

    public enum SignatureModifications
    {
        [Description("No modification")]
        None,

        [Description("Public key modified")]
        E,

        [Description("Message modified")]
        Message,

        [Description("Signature modified")]
        Signature,

        [Description("IR moved from expected location")]
        MoveIr,

        [Description("IR trailer modified from expected value")]
        ModifyTrailer
    }

    public enum PrivateKeyModes
    {
        [Description("standard")]
        Standard,

        [Description("crt")]
        Crt
    }

    public enum PublicExponentModes
    {
        [Description("fixed")]
        Fixed,

        [Description("random")]
        Random
    }
}
