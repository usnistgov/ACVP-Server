namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Represents a single crypto test
    /// </summary>
    public interface ITestCase
    {
        /// <summary>
        /// Unique identifier (scoped to this vector set) for the test.
        /// </summary>
        int TestCaseId { get; set; }
        /// <summary>
        /// Is the test a failure test?
        /// </summary>
        bool FailureTest { get; }
        /// <summary>
        /// When true, crypto operation is performed at least partially from the validator.
        /// When false, all crypto is performed from the generators.
        /// </summary>
        bool Deferred { get; }
    }
}