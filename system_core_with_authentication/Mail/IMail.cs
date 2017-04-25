using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Treshold_Mail.Mail
{
    public interface IMail : IDisposable
    {
        void SendToAdmin(String body, String subject);
    }
}
