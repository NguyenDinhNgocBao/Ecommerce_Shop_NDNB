using System.Net.Mail;
using System.Net;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Repository
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("21t1020237@husc.edu.vn", "kimhqwpxlerzchwg")
            };

            return client.SendMailAsync(
                new MailMessage(from: "21t1020237@husc.edu.vn",
                                to: email,
                                subject,
                                message
                )
            );
        }
    }
}
