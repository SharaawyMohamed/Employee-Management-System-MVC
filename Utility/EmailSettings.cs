using DAL.Entities;
using Demo.PL.Settings;
using MailKit.Security;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;
namespace Demo.PL.Utility
{
	public class EmailSettings// : IMailSettings
	{

		#region Old Way to send Email
		public static void SendEmail(Email email)
		{
			// Mail Servece --> Gemail Servecies
			// Client --> send Email
			// SMTP --> Simple Mail Transfer Protocole ---> the protocole help mp to sent email for user
			//                           host            port
			var Client = new SmtpClient("smtp.gmail.com", 587);
			// Some configurations on cline
			Client.EnableSsl = true;    //                                   from 2-stepverifications -> app passwords
			Client.Credentials = new NetworkCredential("sharawem585@gmail.com", "qmzi ifrj xwyf twpp");
			// step verivications in your mail

			// Send Email from                  to        subject      body	
			Client.Send("sharawem585@gmail.com", email.To, email.Subject, email.Body);
		}
		#endregion

	}
}
