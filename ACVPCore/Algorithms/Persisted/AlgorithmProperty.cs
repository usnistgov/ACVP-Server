using System;

namespace ACVPCore.Algorithms.Persisted
{
	[System.AttributeUsage(AttributeTargets.Property)]
	public sealed class AlgorithmProperty : Attribute
	{
		public string Name { get; set; }

		public AlgorithmPropertyType Type { get; set; }

		public string DefaultValue { get; set; }

		//public Property() { }
		//public Property(string name)
		//{
		//	Name = name;
		//}
	}
}
