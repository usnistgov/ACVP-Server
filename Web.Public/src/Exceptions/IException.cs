using System.Collections.Generic;

namespace Web.Public.Exceptions
{
	public interface IMultiMessageException
	{
		List<string> Errors { get; }
	}
}