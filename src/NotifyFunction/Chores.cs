

using System.Text.Json.Serialization;

namespace ChoreIot
{
    public class Chore
    {
        [JsonPropertyName("zoneId")]
        public string ZoneId { get; set; }
        [JsonPropertyName("choreId")]
        public string ChoreId { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("assignedTo")]
        public string AssignedTo { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }
    }
}