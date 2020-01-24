using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ACVPCore.Algorithms.DataTypes;
using ACVPCore.Models.Capabilities;

namespace ACVPCore.Algorithms.Persisted
{
	public abstract class AlgorithmBase : IPersistedAlgorithm
	{
		public string Name { get; set; }
		public string Mode { get; set; }
		public string Revision { get; set; }
		//public IEnumerable<ICapability> Capabilities => (this.GetType()).GetProperties().Select(x => (ICapability)x.GetValue(this));


		public static BooleanArrayCapability CreateBooleanArrayCapability(IEnumerable<bool> values) => values == null ? null : new BooleanArrayCapability(values);
		public static BooleanCapability CreateBooleanCapability(bool value) => new BooleanCapability { Value = value };
		//public CompositeArrayCapability CreateCompositeArrayCapability()
		public static NumericArrayCapability CreateNumericArrayCapability(IEnumerable<long> values) => values == null ? null : new NumericArrayCapability(values);
		public static NumericCapability CreateNumericCapability(long value) => new NumericCapability { Value = value };
		public static StringArrayCapability CreateStringArrayCapability(IEnumerable<string> values) => values == null || !(values.Any(x => x != null)) ? null : new StringArrayCapability(values);		//Only create if non null input, or there's at least 1 non-null value in the input
		public static StringCapability CreateStringCapability(string value) => value == null ? null : new StringCapability { Value = value };
		public static RangeCapability CreateRangeCapability(DataTypes.Range value) => value == null ? null : new RangeCapability(value);
		public static RangeArrayCapability CreateRangeArrayCapability(IEnumerable<DataTypes.Range> values) => values == null || !(values.Any(x => x != null)) ? null : new RangeArrayCapability(values);       //Only create if non null input, or there's at least 1 non-null value in the input
		public static DomainCapability CreateDomainCapability(Domain value) => value == null ? null : new DomainCapability(value);


		//TODO - get rid of this, put it in CapabilityService or ValidationService or something
		public bool Persist()
		{
			//Do the lookups of the algorithm and each of the properties

			//Persist each of the properties of the class
			Dictionary<string, object> properties = new Dictionary<string, object>();
			foreach (PropertyInfo prop in (this.GetType()).GetProperties())
			{
				//Get the actual property object
				Object property = prop.GetValue(this);

				//If it is a capability type then persist that capability type
				if (property is ICapability)
				{
					//Persist
				}
			}
		}
	}
}
