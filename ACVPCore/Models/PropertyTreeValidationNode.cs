using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models
{
	public class PropertyTreeValidationNode
	{
		public long ID { get; set; }
		public string Name { get; set; }
		public int Level { get; set; }
		public int OrderIndex { get; set; }
		public string Type { get; set; }
	}
}
