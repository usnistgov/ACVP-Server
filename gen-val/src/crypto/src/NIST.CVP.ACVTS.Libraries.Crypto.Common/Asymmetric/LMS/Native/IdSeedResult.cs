namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Represents a new seed and i value used for the creation of child LMS trees.
    /// </summary>
    /// <param name="I">The 16-byte tree identifier to use.</param>
    /// <param name="Seed">The M-byte seed value to use.</param>
    public record IdSeedResult(byte[] I, byte[] Seed);
}
