using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RegistryLibrary.Infrastructure
{
    public delegate void MailError(string errorMessage);

    public class Mailer
    {
        private const string smtpServer = "smtp.gmail.com";
        private const int smtpPort = 587;
        private const string fromAddress = "registry.tma1@gmail.com";
        private const string password = "tm@_2019";


        public static bool SendMail(IEnumerable<string> toAddress, string subject, string body, MailError error, Attachment fileAttachment = null)
        {
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(fromAddress, password),
                EnableSsl = true
            };
            MailMessage mailMessage = new MailMessage();

            //Add recipient address
            foreach (var address in toAddress)
            {
                mailMessage.To.Add(address);
            }

            mailMessage.From = new MailAddress(fromAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            if (fileAttachment != null)
            {
                //Adds attachment to the file
                mailMessage.Attachments.Add(fileAttachment);
            }

            //try and send the mail
            try
            {


                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                error(ex.Message);
                return false;
            }
        }
    }
}
