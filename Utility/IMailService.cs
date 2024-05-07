using DAL.Entities;

namespace Demo.PL.Utility
{
	public interface IMailService
	{
		public void SendMail(Email email);

	}
}
