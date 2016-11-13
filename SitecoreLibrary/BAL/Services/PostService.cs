using System.Net;
using System.Net.Mail;
using SitecoreLibrary.BAL.Contracts;

namespace SitecoreLibrary.BAL.Services
{
    public class PostService:IPostService
    {
        public void SendBookTakenEmail(string eMail, string bookName)
        {
            var fromAddress = new MailAddress("sitecorelibrary2016@gmail.com", "Sitecore Library");
            var toAddress = new MailAddress(eMail, eMail);
            const string fromPassword = "sitecore2016";
            const string subject = " has been taken";
            const string body = "You have just taken book, don't forget to return it";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = bookName + subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public void SendBookReturnedEmail(string eMail)
        {
            var fromAddress = new MailAddress("sitecorelibrary2016@gmail.com", "Sitecore Library");
            var toAddress = new MailAddress(eMail, eMail);
            const string fromPassword = "sitecore2016";
            const string subject = "Book has been returned";
            const string body = "You have just returned book. Hope you liked it";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
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