using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Used to validate a set test vectors for the provided test vector set.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates server generated <see cref="ITestVectorSet"/> against the IUT provided answers.
        /// </summary>
        /// <param name="validateRequest">Contains the ACVP server and IUT provided files for performing validation.</param>
        /// <returns></returns>
        Task<ValidateResponse> ValidateAsync(ValidateRequest validateRequest);
    }
}