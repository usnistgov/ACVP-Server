using System.Collections.Generic;

namespace NIST.CVP.Generation.KAS
{
    public interface ISimpleValidatable
    {
        /// <summary>
        /// Validate the entity, returning an enumerable of validation errors if any are found
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> Validate();
    }
}