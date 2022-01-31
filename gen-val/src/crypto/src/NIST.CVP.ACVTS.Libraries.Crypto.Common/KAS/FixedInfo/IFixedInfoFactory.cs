namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo
{
    /// <summary>
    /// Interface for getting an instance of <see cref="IFixedInfo"/>
    /// </summary>
    public interface IFixedInfoFactory
    {
        /// <summary>
        /// Get an instance of <see cref="IFixedInfo"/>.
        /// </summary>
        /// <returns></returns>
        IFixedInfo Get();
    }
}
