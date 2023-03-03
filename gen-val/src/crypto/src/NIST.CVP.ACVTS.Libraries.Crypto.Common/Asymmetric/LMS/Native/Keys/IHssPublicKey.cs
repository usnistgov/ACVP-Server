using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    /// <summary>
    /// Describes an HSS public key.
    /// </summary>
    public interface IHssPublicKey
    {
        /// <summary>
        /// The number of levels (LMS trees) present in the <see cref="IHssKeyPair"/>.
        /// </summary>
        int Levels { get; }
        /// <summary>
        /// The public key of the HSS scheme -> u32str(L) || pub[0]
        /// </summary>
        Task<byte[]> Key { get; }
        /// <summary>
        /// The signature whose message is the public key of the current tree (L),
        /// signed with the previous tree's private key (L-1).  Signatures that *MAY* be present at sig[L-1]
        /// is the signature for the most recently signed message from the HSS.
        /// </summary>
        Task<byte[][]> Signatures { get; }
    }
}
