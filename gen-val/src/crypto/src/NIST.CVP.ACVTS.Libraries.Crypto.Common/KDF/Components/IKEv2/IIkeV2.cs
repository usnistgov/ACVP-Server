using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv2
{
    /// <summary>
    /// Describes IKEv2 operations
    /// </summary>
    public interface IIkeV2
    {
        /// <summary>
        /// partial IKEv2 generation for retrieving up through the DKM from the specified parameters.
        /// </summary>
        /// <param name="ni">The initiator nonce.</param>
        /// <param name="nr">The responder nonce.</param>
        /// <param name="gir">The shared secret.</param>
        /// <param name="spii">The initiator security parameter index.</param>
        /// <param name="spir">The responder security parameter index.</param>
        /// <param name="dkmLength">The length of the key to derive.</param>
        /// <returns></returns>
        BitString GenerateDkmIke(BitString ni, BitString nr, BitString gir, BitString spii, BitString spir, int dkmLength);
        /// <summary>
        /// Full IKEv2 generation
        /// </summary>
        /// <param name="ni">The initiator nonce.</param>
        /// <param name="nr">The responder nonce.</param>
        /// <param name="gir">The shared secret.</param>
        /// <param name="girNew">The secret that is used for the child KDF.</param>
        /// <param name="spii">The initiator security parameter index.</param>
        /// <param name="spir">The responder security parameter index.</param>
        /// <param name="dkmLength">The length of the key to derive.</param>
        /// <returns></returns>
        IkeResult GenerateIke(BitString ni, BitString nr, BitString gir, BitString girNew, BitString spii, BitString spir, int dkmLength);
    }
}
