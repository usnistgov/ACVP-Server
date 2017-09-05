using NIST.CVP.Crypto.DSA.Enums;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA
{
    /// <summary>
    /// Interface for obtaining a <see cref="IDsa"/> with all dependencies.
    /// </summary>
    public interface IDsaFactory
    {
        /// <summary>
        /// Returns a <see cref="IDsa"/> as a concrete type.
        /// </summary>
        /// <param name="dsaAlgorithm">The Algorithm used for the DSA isntance</param>
        /// <param name="hashFunction">The hash information to be used.</param>
        /// <returns></returns>
        IDsa<
                IDomainParametersGenerateRequest, IDomainParametersGenerateResult, 
                IDomainParametersValidateRequest, IDomainParametersValidateResult, 
                IDsaDomainParameters, IKeyPairGenerateResult, IDsaKeyPair, IKeyPairValidateResult
            >
            GetDsaInstance(DsaAlgorithm dsaAlgorithm, HashFunction hashFunction);
    }
}