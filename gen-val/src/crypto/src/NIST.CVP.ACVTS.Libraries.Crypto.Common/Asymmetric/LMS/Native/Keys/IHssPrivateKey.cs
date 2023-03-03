using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    /// <summary>
    /// Describes a HSS private key.
    /// </summary>
    public interface IHssPrivateKey
    {
        /// <summary>
        /// The total number of levels to the HSS.
        /// </summary>
        int Levels { get; }

        /// <summary>
        /// The LMS key pairs making up full HSS.
        /// </summary>
        Task<ILmsKeyPair[]> Keys { get; }
    }
}
