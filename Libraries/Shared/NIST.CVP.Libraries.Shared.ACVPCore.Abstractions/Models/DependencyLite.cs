using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class DependencyLite
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
	}
}
