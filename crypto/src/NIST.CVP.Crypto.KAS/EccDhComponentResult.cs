using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the result of a <see cref="IEccDhComponent"/> invocation.
    /// </summary>
    public class EccDhComponentResult : ICryptoResult
    {
        /// <summary>
        /// Success constructor
        /// </summary>
        /// <param name="sharedSecretZ">The generated shared secret Z</param>
        public EccDhComponentResult(BitString sharedSecretZ)
        {
            SharedSecretZ = sharedSecretZ;
        }

        /// <summary>
        /// Error constructor
        /// </summary>
        /// <param name="errorMessage">Error information</param>
        public EccDhComponentResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// The shared secret Z
        /// </summary>
        public BitString SharedSecretZ { get; }
        /// <summary>
        /// The error message associated with the failed crypto invoke.
        /// </summary>
        public string ErrorMessage { get; }
        /// <summary>
        /// indicates success/failure of Kas operation
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}