using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Tap.Data
{
    public class MailHelper
    {
        public static bool SendEmail(string From, string To,string UserName, string Password, string Subject, string Body)
        {
            if (!string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To) && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    MailMessage mail = new MailMessage(From, To);
                    SmtpClient client = new SmtpClient();
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Credentials = new System.Net.NetworkCredential(UserName, Password);
                    client.EnableSsl = false;
                    client.Host = "smtp.gmail.com";
                    mail.Subject = Subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    client.Send(mail);
                    return true;
                }
                catch 
                {
                   // LogHelper.LogException(ex);
                    return false;
                }
            }
            return false;
        }
    }
}
