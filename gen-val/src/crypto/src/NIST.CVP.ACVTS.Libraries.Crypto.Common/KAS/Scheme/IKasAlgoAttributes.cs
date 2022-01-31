namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
{
    /// <summary>
    /// The attributes such as scheme and parameter set for a kas scheme.
    /// These values set up bounds for the kas instance such as:
    ///     security strengths
    ///     availability of hashing/MACing algorithms
    ///     the scheme used (which signifies which parties are responsible for contributing which data to the key agreement.
    /// </summary>
    public interface IKasAlgoAttributes
    {
    }
}
