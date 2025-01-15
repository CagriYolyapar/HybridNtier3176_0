using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Tools
{
    public static class MailService
    {

        //testemail3172@gmail.com

        //rvzhpxwpegickwtq


        public static void Send(string receiver,string password = "rvzhpxwpegickwtq",string body="Test mesajıdır",string subject = "Email Testi",string sender = "testemail3172@gmail.com")
        {
            MailAddress senderEmail = new(sender);
            MailAddress receiverEmail = new(receiver);

            //Bizim Email işlemlerimiz SMTP'ye göre yapılır...
            //smtp.office365.com , smtp-mail.outlook.com
            //Kullandıgınız hesabın baska uygulamalar tarafından mesaj gönderme güvenlik protokolünü acmanız lazım...Meselam bizim bizimkini 2 step verification üzerinden bir sifre generate ederk uygun hale getirerek..

            SmtpClient smtp = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address,password)
            };


            using (MailMessage message = new MailMessage(senderEmail, receiverEmail)
            {
                 Subject = subject,
                 Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
