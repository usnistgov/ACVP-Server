using System;
using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore
{
	static public class ParsingHelper
	{
		public static string[] _falseStrings = { "false", "0", "no" };
		public static string[] _trueStrings = { "true", "1", "yes" };

		/// <summary>
		/// Normalize the multitude of ways we've seen the files indicate true and false and return a proper boolean
		/// </summary>
		/// <param name="value">The string we hope to convert to a boolean</param>
		/// <returns>boolean</returns>
		public static bool ParseValueToBoolean(string value)
		{
			//No value --> false
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}

			if (_trueStrings.Contains(value.ToLower()))         //Anything we recognize as a true string --> true
			{
				value = "true";
			}
			else if (_falseStrings.Contains(value.ToLower()))   //Anything we recognize as a false string --> false
			{
				value = "false";
			}

			//Convert whatever we've normalized it to, or whatever else it might be, to a boolean. If the convert fails, default to false
			bool theBool;
			if (!Boolean.TryParse(value.ToLower(), out theBool))
			{
				theBool = false;
			}

			return theBool;
		}

		public static int ParseValueToInteger(string value)
		{
			int intValue;
			if (!int.TryParse(value, out intValue))
			{
				intValue = 0;   //Default to 0 if can't parse to int
			}
			return intValue;
		}

		public static List<int> StringToListOfInts(string someStringWithInts, string delimiter = " ")
		{
			someStringWithInts = someStringWithInts.Trim();
			string[] parts = someStringWithInts.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			return parts.Select(n => Convert.ToInt32(n)).ToList();
		}

		public static List<KeyValuePair<string, string>> GetLinesByKeys(List<KeyValuePair<string, string>> kvp, List<string> keys)
		{
			return kvp.Where(x => keys.Any(s => s == x.Key)).ToList();
		}

		public static string GetValueStringFromSummaryLinesByKey(IEnumerable<string> lines, string key)
		{
			//return lines.SingleOrDefault(l => l.StartsWith(key))?.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)?[1].Trim();

			//Select applies the split to all the lines, SingleOrDefault gets the line that matches the key and has 2 members, then get and trim the second member. Have to split first in case the key is indented (as it in in some cases)
			return lines.Select(l => l.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).SingleOrDefault(x => x[0].StartsWith(key) && x.Length == 2)?[1].Trim();
		}

		public static string GetValueStringFromSummaryLine(string line)
		{
			//Get the value if there are 2 parts, otherwise return null
			string[] parts = line?.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			if (parts?.Length == 2) return parts[1].Trim();
			else return null;
		}

		public static string ZeroStringToNull(string value)
		{
			return value == "0" ? null : value;
		}
	}
}