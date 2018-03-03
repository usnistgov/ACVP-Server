using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.Common.KDF.Components.AnsiX963
{
    public interface IAnsiX963Factory
    {
        IAnsiX963 GetInstance(HashFunction hashFunction);
    }
}