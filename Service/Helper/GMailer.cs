using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helper
{
    public static class GMailer
    {
        public static void SendMail(VmContact model)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress("yourmail@gmail.com"));  // replace with valid value 
            message.From = new MailAddress(model.Email);  // replace with valid value
            message.Subject = model.Subject;
            message.Body = "Contact Request From: "+model.Name+"\nSubject: "+model.Subject+"\nMobile: "+model.MobileNumber+"\nEmail: "+model.Email+"\nMessage: "+model.Message;
            message.IsBodyHtml = false;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "yourmail@gmail.com",  // replace with valid value
                    Password = "yourpassword"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }
    }
}
