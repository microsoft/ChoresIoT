using System.Collections.Generic;

namespace ChoreIot
{
    public class ChoreListData
    {
        public List<Chore> Chores { get; set; } = new List<Chore>();
        public List<Assignee> Assignees { get; set; } = new List<Assignee>();
    }
}