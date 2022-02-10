using System;
using System.Threading.Tasks;
using Azure.Communication.Sms;

namespace ChoreIot
{
    internal class AzureCommunicationService
    {
        internal static async Task<string> SendSmsMessage(Chore chore, Assignee assignee)
        {
            SmsClient smsclient = new SmsClient(Environment.GetEnvironmentVariable("ACSConnectionString"));

            string source = Environment.GetEnvironmentVariable("FromNumber");
            
            string destination = assignee.Phone;

            var response = await smsclient.SendAsync(
                from: source, 
                to: destination, 
                message: chore.Message,
                options: new SmsSendOptions(enableDeliveryReport: true));

            return response.Value.MessageId;
        }
    }
}