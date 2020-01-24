using System;
using ACVPCore.Models.Capabilities;

namespace ACVPCore.Algorithms.Persisted
{
	[System.AttributeUsage(AttributeTargets.Property)]
	public sealed class Property : Attribute
	{
		public string Name { get; set; }

		public DatabaseCapabilityType Type { get; set; }

		public string DefaultValue { get; set; }

		//public Property() { }
		//public Property(string name)
		//{
		//	Name = name;
		//}
	}
}
