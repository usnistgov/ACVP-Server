using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Generation.KAS.FFC
{
    /// <summary>
    /// Interface for retrieving a PQG
    /// </summary>
    public interface IPqgProvider
    {
        /// <summary>
        /// Gets a PQG based on the P/Q lengths and hash function
        /// </summary>
        /// <param name="p">The P length</param>
        /// <param name="q">The q length</param>
        /// <param name="hashFunction">The hash function in the underlying DSA generation</param>
        /// <returns></returns>
        FfcDomainParameters GetPqg(int p, int q, HashFunction hashFunction);
    }
}