using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChoreIot
{
    public class ChoreListData
    {
        [JsonPropertyName("chores")]
        public List<Chore> Chores { get; set; } = new List<Chore>();
        [JsonPropertyName("assignees")]
        public List<Assignee> Assignees { get; set; } = new List<Assignee>();
    }
}