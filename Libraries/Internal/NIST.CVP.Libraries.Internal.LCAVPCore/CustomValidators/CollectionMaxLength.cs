using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.CustomValidators
{
	public class CollectionMaxLength : ValidationAttribute
	{
		private int _maxLength;

		public CollectionMaxLength(int maxLength)
		{
			_maxLength = maxLength;
		}

		public override bool IsValid(object value)
		{
			return (value as IList)?.Count <= _maxLength;
		}
	}
}
