using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CarSelect.Services
{
    public class EmailService
    {
        private static string _senderEmail = "ivanavtopodbor@yandex.com";
        private static string _senderPassword = "nqhufudmkfsmiqsj";

        public static bool SendEmail(string recepient, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.yandex.com", 25);
            smtpClient.EnableSsl = true;
            var basicCredential = new NetworkCredential(_senderEmail, _senderPassword);
            smtpClient.Credentials = basicCredential;

            // Создание объекта MailMessage
            MailMessage mailMessage = new MailMessage(_senderEmail, recepient, subject, body);

            try
            {
                // Отправка письма
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}
