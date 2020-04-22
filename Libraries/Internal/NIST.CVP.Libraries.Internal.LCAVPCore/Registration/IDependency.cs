using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public interface IDependency
	{
		string Url { get; set; }
		string Type { get; set; }
		string Name { get; set; }
		string SWID { get; set; }
		string CPE { get; set; }
		string Manufacturer { get; set; }
		string Family { get; set; }
		string Series { get; set; }
		List<string> Features { get; set; }
		string Description { get; }
	}
}
