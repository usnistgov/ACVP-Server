using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace NIST.CVP.Email
{
	public class Mailer : IMailer
	{
		private readonly EmailConfiguration _emailConfiguration;
		private readonly ILogger<Mailer> _logger;

		public Mailer(IOptions<EmailConfiguration> emailConfiguration, ILogger<Mailer> logger)
		{
			_emailConfiguration = emailConfiguration.Value;
			_logger = logger;
		}

		public bool Send(string subject, string body, string toAddress)
		{
			return Send(subject, body, _emailConfiguration.DefaultFromAddress, new List<string> { toAddress });
		}

		public bool Send(string subject, string body, string fromAddress, List<string> toAddresses = null, List<string> ccAddresses = null, List<string> bccAddresses = null)
		{
			return Send(subject, body, null, fromAddress, toAddresses, ccAddresses, bccAddresses);
		}

		public bool Send(string subject, string body, List<string> attachments, string fromAddress, List<string> toAddresses = null, List<string> ccAddresses = null, List<string> bccAddresses = null)
		{
			//Immediately exit with success if email not enabled
			if (!_emailConfiguration.IsEnabled) return true;

			//Don't try to send if no recipients
			if (!((toAddresses?.Any() ?? false) || (ccAddresses?.Any() ?? false) || (bccAddresses?.Any() ?? false)))
			{
				return false;
			}

			//Use a BodyBuilder to build the body, so we can handle the attachments
			var bodyBuilder = new BodyBuilder
			{
				TextBody = body
			};
			attachments?.ForEach(x => bodyBuilder.Attachments.Add(x));

			//Build the message
			var message = new MimeMessage
			{
				Subject = subject,
				Body = bodyBuilder.ToMessageBody()
			};

			//Add the sender and recipients
			message.From.Add(new MailboxAddress(fromAddress));
			toAddresses?.ForEach(x => message.To.Add(new MailboxAddress(x)));
			ccAddresses?.ForEach(x => message.Cc.Add(new MailboxAddress(x)));
			bccAddresses?.ForEach(x => message.Bcc.Add(new MailboxAddress(x)));

			//Send it
			bool isSuccess = true;
			using var client = new SmtpClient();
			try
			{
				client.Connect(_emailConfiguration.Host, _emailConfiguration.Port);
				client.Send(message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				isSuccess = false;
			}
			finally
			{
				//Apparently need to explicitly disconnect
				client.Disconnect(true);
			}

			return isSuccess;
		}
	}
}
