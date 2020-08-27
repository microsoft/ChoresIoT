using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace ChoreIot
{
    public class Functions
    {
        private readonly ChoreService _choreService;
        public Functions(ChoreService choreService)
        {
            _choreService = choreService;
        }

        [FunctionName(nameof(negotiate))]
        public SignalRConnectionInfo negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "heatmap")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            req.HttpContext.Response.Headers.Add("Content-Type", "application/json");
            return connectionInfo;
        }

        [FunctionName(nameof(Notify))]
        public async Task Notify(
            [BlobTrigger("chores/chores.json", Connection = "AzureWebJobsStorage")] Stream myBlob,
            [SignalR(HubName = "heatmap")] IAsyncCollector<SignalRMessage> heatmap,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            StreamReader reader = new StreamReader(myBlob);
            string choresJson = reader.ReadToEnd();
            var chores = System.Text.Json.JsonSerializer.Deserialize<ChoreListData>(choresJson);

            await _choreService.ProcessChores(chores,
                choreUpdated: async (chore) =>
                {
                    await heatmap.AddAsync(new SignalRMessage
                    {
                        Target = "choreStatusUpdated",
                        Arguments = new[] { chore }
                    });
                },
                beforeSendSms: async (chore) =>
                {
                    await heatmap.AddAsync(new SignalRMessage
                    {
                        Target = "textingChoreAssignee",
                        Arguments = new[] { chore }
                    });
                },
                afterSendSms: async (chore) =>
                {
                    await heatmap.AddAsync(new SignalRMessage
                    {
                        Target = "choreAssigneeTexted",
                        Arguments = new[] { chore }
                    });
                });
        }

        [FunctionName(nameof(HandleSmsTextDeliveredEvent))]
        public async Task<IActionResult> HandleSmsTextDeliveredEvent(
               [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
               [SignalR(HubName = "heatmap")] IAsyncCollector<SignalRMessage> heatmap,
               ILogger log)
        {
            string messageId = req.Query["messageId"];

            await heatmap.AddAsync(new SignalRMessage
            {
                Target = "smsMessageReceived",
                Arguments = new[] { messageId }
            });

            return new OkObjectResult("");
        }
    }
}