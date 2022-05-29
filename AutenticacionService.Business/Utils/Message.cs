using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutenticacionService.Business.Utils
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x, x)));
            Subject = subject;
            Content = content;
        }
        public Message(string to, string subject, string content)
        {
            To = new List<MailboxAddress> { new MailboxAddress(to, to) };
            Subject = subject;
            Content = content;
        }
    }
}
