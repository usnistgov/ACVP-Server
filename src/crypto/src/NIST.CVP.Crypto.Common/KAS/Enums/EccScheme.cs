using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum EccScheme
    {
        [Description("fullUnified")]
        FullUnified,
        [Description("fullMqv")]
        FullMqv,
        [Description("ephemeralUnified")]
        EphemeralUnified,
        [Description("onePassUnified")]
        OnePassUnified,
        [Description("onePassMqv")]
        OnePassMqv,
        [Description("onePassDh")]
        OnePassDh,
        [Description("staticUnified")]
        StaticUnified,
        [Description("componentTest")]
        ComponentTest
    }
}