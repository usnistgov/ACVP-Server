namespace NIST.CVP.Generation.Core.Enums
{
    /// <summary>
    /// Represents the different "audiences" that receive JSON files that represents a <see cref="ITestVectorSet{TTestGroup}"/>
    /// </summary>
    public enum Projection
    {
        /// <summary>
        /// The server's projection - includes all properties
        /// </summary>
        Server,
        /// <summary>
        /// The IUT's projection - includes properties that convey a cryptographic question, 
        /// without providing the answer.
        /// </summary>
        Prompt,
        /// <summary>
        /// The IUT's (intended) response projection - 
        /// includes answers to the "questions" posed in the prompt file.
        /// </summary>
        Result
    }
}