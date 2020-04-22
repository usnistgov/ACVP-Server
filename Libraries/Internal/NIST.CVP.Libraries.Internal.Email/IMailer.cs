using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.Email
{
	public interface IMailer
	{
		bool Send(string subject, string body, string toAddress);
		bool Send(string subject, string body, List<string> attachments, string fromAddress, List<string> toAddresses = null, List<string> ccAddresses = null, List<string> bccAddresses = null);
		bool Send(string subject, string body, string fromAddress, List<string> toAddresses = null, List<string> ccAddresses = null, List<string> bccAddresses = null);
	}
}