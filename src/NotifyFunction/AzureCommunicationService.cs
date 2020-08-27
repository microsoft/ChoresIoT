using System;
using System.Threading.Tasks;
using Azure.Communication.Sms;
using Azure.Communication.Sms.Models;

namespace ChoreIot
{
    internal class AzureCommunicationService
    {
        internal static async Task<string> SendSmsMessage(Chore chore, Assignee assignee)
        {
            SmsClient sms = new SmsClient(Environment.GetEnvironmentVariable("ACSConnectionString"));

            Azure.Communication.PhoneNumber source = 
                new Azure.Communication.PhoneNumber(Environment.GetEnvironmentVariable("FromNumber"));
            
            Azure.Communication.PhoneNumber destination = 
                new Azure.Communication.PhoneNumber(assignee.Phone);

            var response = await sms.SendAsync(source, destination, chore.Message,
                sendSmsOptions: new SendSmsOptions { EnableDeliveryReport = true });

            return response.Value.MessageId;
        }
    }
}