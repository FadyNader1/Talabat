using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(string To, string Body, string Subject);
    }
}
