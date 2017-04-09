using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Math.Domain
{
    /// <summary>
    /// Represents the minimum and maximum value to a <see cref="IDomainSegment"/>
    /// </summary>
    public class RangeMinMax
    {
        public int Minimum { get; set; }

        public int Maximum { get; set; }
    }
}
