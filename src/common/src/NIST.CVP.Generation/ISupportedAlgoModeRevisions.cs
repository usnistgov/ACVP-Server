using System.Collections.Generic;
using NIST.CVP.Common;

namespace NIST.CVP.Generation
{
    /// <summary>
    /// Used to describe the supported Algorithm, Mode, and Revisions for a specific set of IOC registrations.
    ///
    /// Several algorithms IOC registration can support multiple algorithms, utilizing this interface allows for describing that relationship.
    /// </summary>
    /// <inheritdoc />
    public interface ISupportedAlgoModeRevisions : IRegisterInjections
    {
        IEnumerable<AlgoMode> SupportedAlgoModeRevisions { get; }
    }
}