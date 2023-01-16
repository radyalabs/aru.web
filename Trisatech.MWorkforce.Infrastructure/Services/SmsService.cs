using Trisatech.MWorkforce.Infrastructure.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Trisatech.MWorkforce.Infrastructure.Services
{
    public class SmsService
    {
        private readonly string accountSid;
        private readonly string authToken;
        private readonly string sender;

        public SmsService(string accountSid, string authToken, string sender)
        {
            this.accountSid = accountSid;
            this.authToken = authToken;
            this.sender = sender;
        }

        public SmsResponse SendByTwilio(string to, string bodyMessage)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                to: new PhoneNumber(to),
                from: new PhoneNumber(sender),
                body: bodyMessage);

            if(message != null)
            {
                return new SmsResponse
                {
                    Id = message.Sid,
                    DateSent = message.DateSent,
                    Status = message.Status.ToString()
                };
            }
            else
            {
                throw new InvalidOperationException("SMS Service: Invalid response from server");
            }
        }
    }
}
