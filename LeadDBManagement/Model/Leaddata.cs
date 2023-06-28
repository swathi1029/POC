namespace LeadDBManagement.Model
{
    public class Leaddata
    {
        public string? id { get; set; }
        public Contact? contact { get; set; }
        public string? primaryProduct { get; set; }
        public string? programId { get; set; }
        public Score? score { get; set; }
    }

    public class Contact
    {
        public string? emailId { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
    }

    public class Score
    {
        public string? score { get; set; }
        public string? scoreUpdateTimestamp { get; set; }
        public string? rating { get; set; }
    }

  


}
