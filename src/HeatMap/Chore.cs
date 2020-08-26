namespace HeatMap
{
    public class Chore
    {
        public string ZoneId { get; set; }
        public string ChoreId { get; set; }
        public string Message { get; set; }
        public string AssignedTo { get; set; }
        public int Status { get; set; }
        public int Threshold { get; set; }
        public string SmsStatus { get; set; }
        public string MessageId { get; set; }
    }
}