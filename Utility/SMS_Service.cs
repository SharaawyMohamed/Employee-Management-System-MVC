using DAL.Models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Demo.PL.Utility
{
	public class SMS_Service : ISMS_Service
	{
		private readonly TwilioSettings options;

		public SMS_Service(IOptions<TwilioSettings> _options)
		{
			options = _options.Value;
		}
		public MessageResource SendSMS(SMS sms)
		{
			TwilioClient.Init(options.AccountSID, options.AuthToken);
			var res = MessageResource.Create(
				body:sms.Body,
				to: sms.PhoneNumber	,
				from:new Twilio.Types.PhoneNumber(options.TwilioPhone)
				);
			return res;
		}
	}
}
