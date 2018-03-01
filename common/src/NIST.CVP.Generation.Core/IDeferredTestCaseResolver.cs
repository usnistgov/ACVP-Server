using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Describes functions necessary for completing a deferred crypto operation
    /// </summary>
    /// <typeparam name="TTestGroup">The <see cref="ITestGroup"/></typeparam>
    /// <typeparam name="TTestCase">The <see cref="ITestCase"/></typeparam>
    /// <typeparam name="TCryptoResult">The <see cref="ICryptoResult"/> of the completed crypto operation</typeparam>
    public interface IDeferredTestCaseResolver<in TTestGroup, in TTestCase, out TCryptoResult>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
        where TCryptoResult : ICryptoResult
    {
        /// <summary>
        /// Using the provided <see cref="serverTestGroup"/>, <see cref="serverTestCase"/>, 
        /// and <see cref="iutTestCase"/>, complete and return a <see cref="ICryptoResult"/>
        /// </summary>
        /// <param name="serverTestGroup">Group information to be utilized in the crypto</param>
        /// <param name="serverTestCase">The pre-generated server information to use in the crypto operation</param>
        /// <param name="iutTestCase">The IUTs provided information necessary to complete the crypto.</param>
        /// <returns>The completed crypto result</returns>
        TCryptoResult CompleteDeferredCrypto(
            TTestGroup serverTestGroup, 
            TTestCase serverTestCase,
            TTestCase iutTestCase
        );
    }
}