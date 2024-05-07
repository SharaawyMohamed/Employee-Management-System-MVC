using DAL.Entities;

namespace Demo.PL.Utility
{
	public interface IMailSettings
	{
		public void SendMail(Email email);
	}
}
