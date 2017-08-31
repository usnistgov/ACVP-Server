namespace NIST.CVP.Crypto.KAS.KDF
{
    /// <summary>
    /// Returns an instance of <see cref="IOtherInfo"/>
    /// </summary>
    public interface IOtherInfoFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="IOtherInfo"/> with the specified pattern
        /// </summary>
        /// <param name="otherInfoPattern"></param>
        /// <param name="otherInfoLength"></param>
        /// <returns></returns>
        IOtherInfo GetInstance(string otherInfoPattern, int otherInfoLength);
    }
}