using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums
{
    public enum SignatureModifications
    {
        [Description("No modification")]
        None,

        [Description("Key modified")]
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
}
