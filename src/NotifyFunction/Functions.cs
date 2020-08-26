using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
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

        // [FunctionName("NotifyTimer")]
        // public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        // {
        //     log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        //     await ProcessChores();
        // }

        [FunctionName(nameof(negotiate))]
        public SignalRConnectionInfo negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "heatmap")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            return connectionInfo;
        }

        [FunctionName(nameof(NotifyHttp))]
        public async Task<IActionResult> NotifyHttp(
               [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
               [SignalR(HubName = "heatmap")] IAsyncCollector<SignalRMessage> heatmap,
               ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _choreService.ProcessChores(
                choreUpdated: (chore) =>
                {
                    heatmap.AddAsync(new SignalRMessage
                    {
                        Target = "choreStatusUpdated",
                        Arguments = new[] { chore }
                    });
                },
                beforeSendSms: (chore) => 
                {
                    heatmap.AddAsync(new SignalRMessage
                    {
                        Target = "textingChoreAssignee",
                        Arguments = new[] { chore }
                    });
                },
                afterSendSms: (chore) => 
                {
                    heatmap.AddAsync(new SignalRMessage
                    {
                        Target = "choreAssigneeTexted",
                        Arguments = new[] { chore }
                    });
                });

            return new OkObjectResult("");
        }
    }
}