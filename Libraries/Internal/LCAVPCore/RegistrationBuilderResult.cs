using System.Collections.Generic;

namespace LCAVPCore
{
	public class RegistrationBuilderResult
	{
		public bool Success
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		public string RegistrationJson { get; set; }

		public List<string> Errors { get; set; } = new List<string>();
	}
}
