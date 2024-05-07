using DAL.Entities;
using Demo.PL.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit;
namespace Demo.PL.Utility
{
	public class MailService : IMailService
	{
		private readonly MailSettings mailSetting;

		public MailService(IOptions<MailSettings> _mailSetting)
		{
			mailSetting = _mailSetting.Value;
		}
		public void SendMail(Email email)
		{
			var mail = new MimeMessage()
			{
				Sender = MailboxAddress.Parse(mailSetting.Email),
				Subject = email.Subject,

			};
			mail.To.Add(MailboxAddress.Parse(email.To));
			var builder = new BodyBuilder();
			builder.TextBody = email.Body;
			mail.Body = builder.ToMessageBody();
			mail.From.Add(new MailboxAddress(mailSetting.DisplayName, mailSetting.Email));
			using var smtp = new SmtpClient();
			smtp.Connect(mailSetting.Host, mailSetting.Port,MailKit.Security.SecureSocketOptions.StartTls);
			smtp.Authenticate(mailSetting.Email, mailSetting.Password);
			smtp.Send(mail);
			smtp.Disconnect(true);
		}
	}
}
