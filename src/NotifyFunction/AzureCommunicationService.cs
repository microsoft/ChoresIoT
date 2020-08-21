using System;
using Azure.Communication.Sms;
using Azure.Communication.Sms.Models;

namespace ChoreIot
{
    internal class AzureCommunicationService
    {
        internal static void SendSmsMessage(Chore chore, Assignee assignee)
        {
            SmsClient sms = new SmsClient(Environment.GetEnvironmentVariable("ACSConnectionString"));

            Azure.Communication.PhoneNumber source = 
                new Azure.Communication.PhoneNumber(Environment.GetEnvironmentVariable("FromNumber"));
            
            Azure.Communication.PhoneNumber destination = 
                new Azure.Communication.PhoneNumber(assignee.Phone);

            sms.Send(source, destination, chore.Message,
                sendSmsOptions: new SendSmsOptions { EnableDeliveryReport = true });
        }
    }
}