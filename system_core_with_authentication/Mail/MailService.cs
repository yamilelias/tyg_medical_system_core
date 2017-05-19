using Hangfire;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;

namespace Treshold_Mail.Mail
{
    public class MailService : IMail
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MailService(ApplicationDbContext _context,
                           UserManager<ApplicationUser> _userManager,
                           RoleManager<IdentityRole> _roleManager)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
        }

        public void Dispose()
        {
            
        }

        public void SendToAdmin(string body, string subject)
        {
            var adminRole = _roleManager.Roles.Where(e => e.Name == "Admin").FirstOrDefault();
          
            var emails = _userManager.Users.Where(e => e.Roles.Select(r => r.RoleId).Contains(adminRole.Id)).ToList();
            

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TyG SysAdmin", "hostmaster@arturozamora.net"));

            emails.ForEach(e =>
            {
                Debug.WriteLine($"Name: {e.Name} \nLast name: {e.LastName} \nEmail:{e.Email}");
                message.To.Add(new MailboxAddress($"{e.Name} {e.LastName}", $"{e.Email}"));
            });
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            if (emails.Count > 0)
                SendEmail(message);
        }

        public void SendToSupervisor(string body, string subject)
        {
            var supervisorRole = _roleManager.Roles.Where(e => e.Name == "Supervisor").FirstOrDefault();
            var emails = _userManager.Users.Where(e => e.Roles.Select(r => r.RoleId).Contains(supervisorRole.Id)).ToList();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TyG SysAdmin", "hostmaster@arturozamora.net"));

            emails.ForEach(e =>
            {
                message.To.Add(new MailboxAddress($"{e.Name} {e.LastName}", $"{e.Email}"));
            });
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            if(emails.Count > 0)
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
