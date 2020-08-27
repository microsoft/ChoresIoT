using System.Text.Json.Serialization;

namespace ChoreIot
{
    public class Assignee
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
    }
}