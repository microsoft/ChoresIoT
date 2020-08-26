using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace ChoreIot
{
    public class ChoreService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChoreService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ChoreListData> GetChores()
        {
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(Environment.GetEnvironmentVariable("ChoresApi"));
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ChoreListData>(responseBody, options);
        }

        public async Task ProcessChores(Action<Chore> choreUpdated = null,
            Action<Chore> beforeSendSms = null,
            Action<Chore> afterSendSms = null)
        {
            if (Convert.ToBoolean(Environment.GetEnvironmentVariable("Run")))
            {
                var choreData = await GetChores();

                foreach (var chore in choreData.Chores)
                {
                    choreUpdated?.Invoke(chore);

                    // if the status is greater than the threshold, sound an alarm
                    if (chore.Status >= chore.Threshold)
                    {
                        beforeSendSms?.Invoke(chore);

                        var assignedTo = choreData.Assignees.First(x => x.Name == chore.AssignedTo);
                        var messageId = AzureCommunicationService.SendSmsMessage(chore, assignedTo);

                        chore.MessageId = messageId;

                        afterSendSms?.Invoke(chore);
                    }
                }
            }
        }
    }
}