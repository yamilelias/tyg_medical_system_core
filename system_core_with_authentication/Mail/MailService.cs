using Hangfire;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Treshold_Mail.Mail
{
    public class MailService : IMail
    {
        public void Dispose()
        {
            
        }

        public void SendToAdmin(string body, string subject)
        {
            bool sent = false;
            String consoleMail = "sysadmin@tygmedical.com.mx";
            String adminMail = "a01560815@itesm.mx";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TyG SysAdmin", "hostmaster@arturozamora.net"));
            message.To.Add(new MailboxAddress("Arturo Zamora", adminMail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            SendEmail(message);
        }

        private void SendEmail(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect("a2plcpnl0913.prod.iad2.secureserver.net", 465, true);
                //client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: since we don't have an OAuth2 token, disable 	
                // the XOAUTH2 authentication mechanism. 
                client.Authenticate("hostmaster@arturozamora.net", "E83zktZD5zHl");
                client.Send(message);
                client.Disconnect(true);
                Debug.WriteLine("Message sent");
            }
        }
    }
}
