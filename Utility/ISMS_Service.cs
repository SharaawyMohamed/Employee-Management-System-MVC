using DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Demo.PL.Utility
{
	public interface ISMS_Service
	{
		MessageResource SendSMS(SMS sms);

	}
}
