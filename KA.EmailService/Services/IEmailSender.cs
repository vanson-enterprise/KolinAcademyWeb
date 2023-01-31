using KA.EmailService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.EmailService.Services
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage message);
        Task SendEmailAsync(EmailMessage message);
    }
}
