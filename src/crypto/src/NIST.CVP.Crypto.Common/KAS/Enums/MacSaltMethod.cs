namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum MacSaltMethod
    {
        /// <summary>
        /// No salt is used.
        /// </summary>
        None,
        /// <summary>
        /// The default salt of all 0 bits is used.
        /// </summary>
        Default,
        /// <summary>
        /// The salt is randomly generated and included in the otherInput
        /// </summary>
        Random
    }
}