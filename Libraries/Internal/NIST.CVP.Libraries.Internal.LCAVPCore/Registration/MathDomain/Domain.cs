using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain
{
	public class Domain : List<object>
	{
		public NIST.CVP.Libraries.Shared.Algorithms.DataTypes.Domain ToCoreDomain()
		{
			if (this == null) return null;

			var result = new NIST.CVP.Libraries.Shared.Algorithms.DataTypes.Domain();

			foreach (var segment in this)
			{
				if (segment is int)
				{
					result.Segments.Add(new NumericSegment { Value = (int)segment });
				}
				else
				{
					//Must be a range
					result.Segments.Add(((Range)segment).ToCoreRange());
				}
			}

			return result;
		}

		public List<long> ToLongList()
		{
			//Better only use this when the segments aren't ranges...
			return this.Select(x => (long)x).ToList();
		}
	}
}
