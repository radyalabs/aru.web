namespace Trisatech.MWorkforce.Infrastructure.Models
{
    public class SmsResponse
    {
        public SmsResponse()
        {
            Id = Guid.NewGuid().ToString();
            DateSent = DateTime.Now;
            Status = string.Empty;
        }
        public string Id { get; set; }
        public DateTime? DateSent { get; set; }
        public string Status { get; set; }
    }
}
