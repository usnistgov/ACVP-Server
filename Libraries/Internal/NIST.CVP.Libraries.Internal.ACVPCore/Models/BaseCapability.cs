using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public abstract class BaseCapability
	{
		public bool Historical { get; set; }
		public string Label { get; set; }
		public string UnitsLabel { get; set; }
		public abstract string HTML { get; }
		public virtual string ValueDisplayHTML { get; }
		public virtual long NumericSortValue { get; set; }
	}
}
