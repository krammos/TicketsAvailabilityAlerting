namespace TicketsAvailabilityAlerting.Models
{
    public class MailSettings
    {
        public string MailServer { get; set; }
        public int MailServerPort { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}