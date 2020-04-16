using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Email
{
	public class EmailConfiguration
	{
		public bool IsEnabled { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string UserId { get; set; }
		public string Password { get; set; }
		public string DefaultFromAddress { get; set; }
	}
}
