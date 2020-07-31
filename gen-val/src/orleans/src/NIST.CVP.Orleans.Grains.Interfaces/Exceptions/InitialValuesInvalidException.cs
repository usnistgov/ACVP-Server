using System;

namespace NIST.CVP.Orleans.Grains.Interfaces.Exceptions
{
	public class InitialValuesInvalidException : Exception
	{
		private const string ERROR_MESSAGE =
			"The values passed into the grain are invalid for the testing scenario.  Invoke the grain again with new parameters.";

		public InitialValuesInvalidException()
			: base(ERROR_MESSAGE) { }
	}
}