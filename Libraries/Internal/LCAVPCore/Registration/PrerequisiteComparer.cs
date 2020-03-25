using System.Collections.Generic;

namespace LCAVPCore.Registration
{
	public class PrerequisiteComparer : EqualityComparer<Prerequisite>
	{
		private const char SEPARATOR = (char)31;

		public override bool Equals(Prerequisite x, Prerequisite y)
		{
			return x.Algorithm == y.Algorithm
				&& ((x.ValidationRecordID == null && x.SubmissionID == null && y.ValidationRecordID == null && y.SubmissionID == null)
					|| (x.ValidationRecordID == y.ValidationRecordID && x.ValidationRecordID != null)
					|| (x.SubmissionID == y.SubmissionID && x.SubmissionID != null));
		}

		public override int GetHashCode(Prerequisite obj)
		{
			return (obj.Algorithm + SEPARATOR + obj.ValidationRecordID + SEPARATOR + obj.SubmissionID).GetHashCode();
		}
	}
}
