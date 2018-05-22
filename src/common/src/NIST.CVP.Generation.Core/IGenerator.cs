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
        /// <param name="requestFilePath">The full file path to the registration json</param>
        /// <returns></returns>
        GenerateResponse Generate(string requestFilePath);
    }
}