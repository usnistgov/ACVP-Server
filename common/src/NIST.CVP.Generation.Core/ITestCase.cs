using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Represents a single crypto test
    /// </summary>
    public interface ITestCase<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Unique identifier (scoped to this vector set) for the test.
        /// </summary>
        [JsonProperty(PropertyName = "tcId")]
        int TestCaseId { get; set; }
        /// <summary>
        /// The <see cref="ITestCase"/>'s parent <see cref="ITestGroup{TTestCase}"/> used for 
        /// serialization purposes.
        /// </summary>
        [JsonIgnore]
        TTestGroup ParentGroup { get; set; }
        /// <summary>
        /// States if the test is a passing (true) test.
        /// 
        /// Nullable as bool default value is "false", and property not included
        /// for many test types.  Would not want "failure test by default" to be inferred.
        /// </summary>
        bool? TestPassed { get; }
        /// <summary>
        /// When true, crypto operation is performed at least partially from the validator.
        /// When false, all crypto is performed from the generators.
        /// </summary>
        bool Deferred { get; }
    }
}