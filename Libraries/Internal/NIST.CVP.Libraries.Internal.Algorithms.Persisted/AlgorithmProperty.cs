using System;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	[System.AttributeUsage(AttributeTargets.Property)]
	public sealed class AlgorithmProperty : Attribute
	{
		public string Name { get; set; }

		public AlgorithmPropertyType Type { get; set; }

		public bool PrependParentPropertyName { get; set; } = false;

		public string DefaultValue { get; set; }

		public AlgorithmProperty(string name)
		{
			Name = name;
		}

		public AlgorithmProperty() { }
	}
}
