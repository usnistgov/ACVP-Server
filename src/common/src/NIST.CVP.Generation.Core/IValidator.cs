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
        /// <param name="resultPath">Results file (from IUT)</param>
        /// <param name="answerPath">Answer file (server generated)</param>
        /// <returns></returns>
        ValidateResponse Validate(string resultPath, string answerPath);
    }
}