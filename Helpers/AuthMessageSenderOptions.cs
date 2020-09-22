using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fermezza.Helpers
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }

        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpUseSSL { get; set; }
    }
}
