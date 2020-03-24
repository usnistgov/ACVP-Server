using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace LCAVPCore.CustomValidators
{
	public class CollectionMinLength : ValidationAttribute
	{
		private int _minLength;

		public CollectionMinLength(int minLength)
		{
			_minLength = minLength;
		}

		public override bool IsValid(object value)
		{
			return (value as IList)?.Count >= _minLength;
		}
	}
}
