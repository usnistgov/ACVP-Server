using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Provides <see cref="ITestGroupGenerator{TParameters}"/>s for a set of parameters.
    /// </summary>
    /// <typeparam name="TParameters">The parameters type.</typeparam>
    public interface ITestGroupGeneratorFactory<in TParameters>
        where TParameters : IParameters
    {
        /// <summary>
        /// Gets a collection of <see cref="ITestGroupGenerator{TParameters}"/>.
        /// Separate group generators are often used for the different test types (AFT, KAT, VOT, etc)
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITestGroupGenerator<TParameters>> GetTestGroupGenerators();
    }
}