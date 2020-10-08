using System.Collections.Generic;

namespace HeatMap
{
    public class ChoreListData
    {
        public List<Chore> Chores { get; set; } = new List<Chore>();
        public List<Assignee> Assignees { get; set; } = new List<Assignee>();
    }
}