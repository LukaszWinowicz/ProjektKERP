using System.Collections.Generic;

namespace Services
{
    public class EmailMessage
    {
        public string SendFrom { get; set; }
        public List<string> SendTo { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public List<string> Files { get; set; }
    }
}