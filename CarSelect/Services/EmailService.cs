using CarSelect.Data;
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

        public static bool SendEmail(Request request)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.yandex.com", 25);
            smtpClient.EnableSsl = true;
            var basicCredential = new NetworkCredential(_senderEmail, _senderPassword);
            smtpClient.Credentials = basicCredential;

            var body =
                "<html>\r\n" +
                "<head>\r\n  " +
                "<style>\r\n    " +
                "body {\r\n" +
                "      font-family: Arial, sans-serif;\r\n" +
                "      margin: 0;\r\n" +
                "      padding: 0;\r\n" +
                "    }\r\n" +
                "    \r\n" +
                "    .container {\r\n" +
                "      max-width: 600px;\r\n" +
                "      margin: 20px auto;\r\n" +
                "      padding: 20px;\r\n" +
                "      background-color: #f2f2f2;\r\n" +
                "      border: 1px solid #ccc;\r\n" +
                "    }\r\n" +
                "    \r\n" +
                "    h1 {\r\n" +
                "      color: #333;\r\n" +
                "      margin-top: 0;\r\n" +
                "    }\r\n    \r\n" +
                "    p {\r\n" +
                "      color: #555;\r\n" +
                "    }\r\n" +
                "    \r\n    .footer {\r\n" +
                "      margin-top: 20px;\r\n" +
                "      color: #999;\r\n" +
                "      font-size: 12px;\r\n" +
                "    }\r\n" +

                ".car-details {\r\n" +
                "      margin-top: 20px;\r\n" +
                "      border-top: 1px solid #ccc;\r\n" +
                "      padding-top: 20px;\r\n" +
                "    }\r\n" +
                "    \r\n" +
                "    .car-details p {\r\n" +
                "      margin-bottom: 10px;\r\n" +
                "    }" +
                "  </style>\r\n" +
                "</head>\r\n" +
                "<body>\r\n" +
                "  <div class=\"container\">\r\n" +
                $"    <h1>Здравствуйте, {request.Client.LastName} {request.Client.FirstName} {request.Client.Patronymic}!</h1>\r\n" +
                "    <p>Мы хотели бы сообщить вам о состоянии заявки на автоподбор.</p>\r\n" +
                "<div class=\"car-details\">\r\n" +
                $"      <p><h2>Подобранный автомобиль: {request.Car}</h2></p>\r\n" +
                $"      <p><strong>Тип кузова:</strong> {request.Car.BodyType.Name}</p>\r\n" +
                $"      <p><strong>Тип привода:</strong> {request.Car.DriveType.Name}</p>\r\n" +
                $"      <p><strong>Тип топлива:</strong> {request.Car.FuelType.Name}</p>\r\n" +
                $"      <p><strong>Тип КПП:</strong> {request.Car.GBType.Name}</p>\r\n" +
                $"      <p><strong>Пробег:</strong> {request.Car.Mileage}</p>\r\n" +
                $"      <p><strong>Цена:</strong> {request.Car.Price}</p>\r\n" +
                $"      <p><strong>Год выпуска:</strong> {request.Car.ReleaseYear}</p>\r\n" +
                $"      <p><strong>Объем двигателя:</strong> {request.Car.EngineVolume}</p>\r\n" +
                $"      <p><strong>Цвет:</strong> {request.Car.Color}</p>\r\n" +
                "    </div>" +
                $"    <h2>Статус заявки: {request.State.Name}</h2>\r\n" +
                "    <p>Если у вас есть вопросы или требуется дополнительная информация, пожалуйста, свяжитесь с нами.</p>\r\n    \r\n" +
                "    <div class=\"footer\">\r\n" +
                "      <p>С уважением,<br>Ваша команда автоподбора</p>\r\n" +
                "    </div>\r\n  " +
                "</div>\r\n" +
                "</body>\r\n" +
                "</html>";

            var recepient = request.Client.Email;
            var subject = "По вашей заявке найден автомобиль";

            // Создание объекта MailMessage
            MailMessage mailMessage = new MailMessage(_senderEmail, recepient, subject, body);
            mailMessage.IsBodyHtml = true;

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
