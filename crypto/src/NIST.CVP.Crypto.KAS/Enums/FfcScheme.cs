namespace NIST.CVP.Crypto.KAS.Enums
{
    public enum FfcScheme
    {
        /// <summary>
        /// C(2e, 2s, FFC DH)
        /// </summary>
        DhHybrid1,
        /// <summary>
        /// C(2e, 2s, FFC MQV)
        /// </summary>
        Mqv2,
        /// <summary>
        /// C(2e, 0s, FFC DH)
        /// </summary>
        DhEphem,
        /// <summary>
        /// C(1e, 2s, FFC DH)
        /// </summary>
        DhHybridOneFlow,
        /// <summary>
        /// C(1e, 2s, FFC MQV)
        /// </summary>
        Mqv1,
        /// <summary>
        /// C(1e, 1s, FFC DH)
        /// </summary>
        DhOneFlow,
        /// <summary>
        /// C(0e, 2s, FFC DH)
        /// </summary>
        DhStatic
    }
}