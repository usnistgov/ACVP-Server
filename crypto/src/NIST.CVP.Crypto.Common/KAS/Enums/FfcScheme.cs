using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum FfcScheme
    {
        /// <summary>
        /// C(2e, 2s, FFC DH)
        /// </summary>
        [Description("dhHybrid1")]
        DhHybrid1,
        /// <summary>
        /// C(2e, 2s, FFC MQV)
        /// </summary>
        [Description("mqv2")]
        Mqv2,
        /// <summary>
        /// C(2e, 0s, FFC DH)
        /// </summary>
        [Description("dhEphem")]
        DhEphem,
        /// <summary>
        /// C(1e, 2s, FFC DH)
        /// </summary>
        [Description("dhHybridOneFlow")]
        DhHybridOneFlow,
        /// <summary>
        /// C(1e, 2s, FFC MQV)
        /// </summary>
        [Description("mqv1")]
        Mqv1,
        /// <summary>
        /// C(1e, 1s, FFC DH)
        /// </summary>
        [Description("dhOneFlow")]
        DhOneFlow,
        /// <summary>
        /// C(0e, 2s, FFC DH)
        /// </summary>
        [Description("dhStatic")]
        DhStatic
    }
}