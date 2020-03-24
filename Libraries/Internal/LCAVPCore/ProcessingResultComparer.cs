using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore
{
	public class ProcessingResultComparer : EqualityComparer<ProcessingResult>
	{
		private const char SEPARATOR = (char)31;

		public override bool Equals(ProcessingResult x, ProcessingResult y)
		{
			return x.Type == y.Type
				&& x.WorkflowType == y.WorkflowType
				&& x.Errors.SequenceEqual(y.Errors)
				&& (x.RegistrationJson ?? "") == (y.RegistrationJson ?? "");
		}

		public override int GetHashCode(ProcessingResult obj)
		{
			return (obj.Type.ToString() + SEPARATOR + obj.WorkflowType.ToString() + SEPARATOR + obj.RegistrationJson + SEPARATOR + string.Join(SEPARATOR.ToString(), obj.Errors)).GetHashCode();
		}
	}
}
