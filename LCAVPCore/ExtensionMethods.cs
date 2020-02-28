using System.Collections.Generic;
using System.Linq;
using LCAVPCore.Registration.MathDomain;

namespace LCAVPCore
{
	public static class ExtensionMethods
	{
		public static void Add<Tkey, TValue>(this IDictionary<Tkey, TValue> target, IEnumerable<KeyValuePair<Tkey, TValue>> source)
		{
			if (source != null)
			{
				foreach (var kvp in source)
				{
					//Make sure it isn't a duplicate key before trying to add - RSA has duplicate values in inf file
					if (!target.ContainsKey(kvp.Key)) target.Add(kvp.Key, kvp.Value);
				}
			}
		}

		public static TValue GetValue<Tkey, TValue>(this IDictionary<Tkey, TValue> dict, Tkey key)
		{
			//This returns the default value for the type if the key is not found
			TValue value;
			dict.TryGetValue(key, out value);
			return value;
		}

		public static ACVPCore.Algorithms.DataTypes.Domain ToCoreDomain(List<int> values)
		{
			var domain = new ACVPCore.Algorithms.DataTypes.Domain();

			foreach(int value in values)
			{
				domain.Segments.Add(new ACVPCore.Algorithms.DataTypes.NumericSegment { Value = value });
			}

			return domain;
		}

		public static List<ACVPCore.Algorithms.DataTypes.Range> ToCoreRangeList(List<Range> values) => values.Select(x => x.ToCoreRange()).ToList();
	}
}