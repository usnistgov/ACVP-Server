using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    /// <summary>
    /// Exposes a means of creating <see cref="IHssKeyPair"/>s.
    /// </summary>
    public interface IHssKeyPairFactory
    {
        ///  <summary>
        ///  Generates a <see cref="IHssKeyPair"/> for the number of levels provided in the <see cref="request"/> length.
        /// 
        ///  https://datatracker.ietf.org/doc/html/rfc8554#section-6.1
        ///  </summary>
        ///  <param name="request">
        /// 		Each <see cref="IHssKeyPairGenerationRequest"/> in the array represents
        /// 		a level in the <see cref="IHssKeyPair"/>.
        ///  </param>
        ///  <param name="hssLevelParameters">The <see cref="LmsMode"/> and <see cref="LmOtsMode"/>s that go into each level of the HSS scheme.</param>
        ///  <param name="randomizerC">The randomizer implementation used for signing messages.</param>
        ///  <param name="i">The initial 16 byte LMS tree identifier (level 0).</param>
        ///  <param name="seed">The initial (m-byte) LMS seed (level 0).</param>
        ///  <returns>The constructed <see cref="IHssKeyPair"/>.</returns>
        Task<IHssKeyPair> GetKeyPair(HssLevelParameter[] hssLevelParameters, ILmOtsRandomizerC randomizerC, byte[] i, byte[] seed);

        /// <summary>
        /// Attempts to repopulate the LMS trees, progressing the Q value starting at the lowest
        /// level LMS tree possible.
        /// </summary>
        /// <param name="keyPair">The key pair to regenerate keys for.</param>
        /// <param name="randomizerC">The randomizer implementation used for signing messages.</param>
        /// <param name="incrementQ">Increment the Q value the provided number of times.</param>
        /// <returns>Returns true if the key pair is still valid for use.</returns>
        Task<bool> RegenerateLmsTreesWhereRequired(IHssKeyPair keyPair, ILmOtsRandomizerC randomizerC, int incrementQ);

        /// <summary>
        /// Generates a <see cref="IHssPrivateKey"/> for the number of levels provided in the <see cref="request"/> length.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-6.1
        /// </summary>
        ///  <param name="hssLevelParameters">The <see cref="LmsMode"/> and <see cref="LmOtsMode"/>s that go into each level of the HSS scheme.</param>
        ///  <param name="i">The initial 16 byte LMS tree identifier (level 0).</param>
        ///  <param name="seed">The initial (m-byte) LMS seed (level 0).</param>
        /// <returns>The constructed <see cref="IHssPrivateKey"/>.</returns>
        IHssPrivateKey GetPrivateKey(HssLevelParameter[] hssLevelParameters, byte[] i, byte[] seed);

        /// <summary>
        /// Generates a <see cref="IHssPublicKey"/> based on the provided <see cref="IHssPrivateKey"/>.
        /// 
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-6.1
        /// </summary>
        /// <param name="privateKey">The <see cref="IHssPrivateKey"/> to generate a <see cref="IHssPublicKey"/> for.</param>
        /// <param name="randomizerC">The randomizer implementation used for signing messages.</param>
        /// <returns>The constructed <see cref="IHssPublicKey"/>.</returns>
        Task<IHssPublicKey> GetPublicKey(IHssPrivateKey privateKey, ILmOtsRandomizerC randomizerC);
    }
}
