using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LCAVPCore.CustomValidators
{
	public class AllowedValues : ValidationAttribute
	{
		private string _allowedValues;

		public AllowedValues(string allowedValues)
		{
			_allowedValues = allowedValues;
		}

		public override bool IsValid(object value)
		{
			//Make sure passed in some allowed values
			if (string.IsNullOrWhiteSpace(_allowedValues)) return false;

			//Make sure the collection we want to test actually has values
			var collection = value as IList;
			if (collection == null) return false;

			//Split the legal values into a collection. Have to do it this slightly complex way to ensure each entry is trimmed
			List<string> allowed = _allowedValues.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();


			//Check that each value in the collection is an allowed value. Since we know we are only working with strings and ints, safe to to a ToString()
			foreach (var item in collection)
			{
				if (!allowed.Contains(item.ToString())) return false;
			}

			//Everything must be good if we got here...
			return true;
		}
	}
}
