using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class EmailSetting
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string EmailSender { get; set; }
        public string SenderPassword { get; set; }

    }
}
