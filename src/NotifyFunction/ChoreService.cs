using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChoreIot
{
    public class ChoreService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<List<Chores>> GetChores()
        {
            HttpResponseMessage response = await client.GetAsync(Environment.GetEnvironmentVariable("ChoresApi"));
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Chores>>(responseBody, options);
        }
    }
}