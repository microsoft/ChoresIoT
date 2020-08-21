using Azure.Communication.Sms;
using Azure.Communication.Sms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChoreIot
{
    public class Functions
    {
        private readonly ChoreService choreService;
        public Functions(ChoreService _choreService)
        {
            choreService = _choreService;
        }

        // [FunctionName("NotifyTimer")]
        // public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        // {
        //     log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        //     await ProcessChores();
        // }

        [FunctionName("NotifyHttp")]
        public async Task<IActionResult> NotifyHttp(
               [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
               ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            await ProcessChores();
            return new OkObjectResult("");
        }

        private async Task ProcessChores()
        {
            if (Convert.ToBoolean(Environment.GetEnvironmentVariable("Run")))
            {
                var choreData = await choreService.GetChores();

                foreach (var chore in choreData.Chores)
                {
                    // if the status is greater than the threshold, sound an alarm
                    if (chore.Status >= chore.Threshold)
                    {
                        var assignedTo = choreData.Assignees.First(x => x.Name == chore.AssignedTo);
                        SendSmsMessage(chore, assignedTo);
                    }
                }
            }
        }

        private void SendSmsMessage(Chore chore, Assignee assignee)
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