using System.IO;
using System.Collections.Generic;

namespace Services
{
    public class SftpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}