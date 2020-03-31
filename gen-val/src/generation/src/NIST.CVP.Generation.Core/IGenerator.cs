using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Used to generate a set test vectors for a provided JSON registration.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Generate <see cref="ITestVectorSet"/> based on an algorithms registration json
        /// </summary>
        /// <param name="generateRequest">The json representing an <see cref="IParameters"/>.</param>
        /// <returns></returns>
        Task<GenerateResponse> GenerateAsync(GenerateRequest generateRequest);
    }
}