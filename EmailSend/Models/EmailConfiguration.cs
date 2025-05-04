
namespace EmailSend.Models
{
    public class EmailConfiguration
    {
        public string? EmailHost { get;  set; }
        public int EmailPort { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmailAddress { get; set; }
        public string? EmailPassword { get; set; }
        public string? LoggingPath { get; set; }
        public string? EmailLogFile { get; set; }

    }
}
