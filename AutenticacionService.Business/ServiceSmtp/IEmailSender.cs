using AutenticacionService.Business.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServiceSmtp
{
    public interface IEmailSender
    {
        Task SendEmail(Message message);
    }
}
