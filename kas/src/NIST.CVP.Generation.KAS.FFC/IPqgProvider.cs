using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Generation.KAS.FFC
{
    /// <summary>
    /// Interface for retrieving a PQG
    /// </summary>
    public interface IPqgProvider
    {
        FfcDomainParameters GetPqg(int p, int q, HashFunction hashFunction);
    }
}