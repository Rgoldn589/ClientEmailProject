namespace CustomerEmail.Models
{
    public class EmailLog
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime LogDate { get; set; }
        public string Result {  get; set; }
    }
}
