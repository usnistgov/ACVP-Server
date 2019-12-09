using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum FfcScheme
    {
        None,
        /// <summary>
        /// C(2e, 2s, FFC DH)
        /// </summary>
        [EnumMember(Value = "dhHybrid1")]
        DhHybrid1,
        /// <summary>
        /// C(2e, 2s, FFC MQV)
        /// </summary>
        [EnumMember(Value = "mqv2")]
        Mqv2,
        /// <summary>
        /// C(2e, 0s, FFC DH)
        /// </summary>
        [EnumMember(Value = "dhEphem")]
        DhEphem,
        /// <summary>
        /// C(1e, 2s, FFC DH)
        /// </summary>
        [EnumMember(Value = "dhHybridOneFlow")]
        DhHybridOneFlow,
        /// <summary>
        /// C(1e, 2s, FFC MQV)
        /// </summary>
        [EnumMember(Value = "mqv1")]
        Mqv1,
        /// <summary>
        /// C(1e, 1s, FFC DH)
        /// </summary>
        [EnumMember(Value = "dhOneFlow")]
        DhOneFlow,
        /// <summary>
        /// C(0e, 2s, FFC DH)
        /// </summary>
        [EnumMember(Value = "dhStatic")]
        DhStatic
    }
}