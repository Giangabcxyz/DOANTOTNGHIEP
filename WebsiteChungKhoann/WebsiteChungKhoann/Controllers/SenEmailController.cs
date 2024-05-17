using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebsiteChungKhoann.Controllers
{
    public class SenEmailController : Controller
    {
        // GET: SenEmail
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendEmail()
        {
            var fromEmailAddress = "your-email@gmail.com";
            var fromEmailDisplayName = "Your Display Name";
            var fromEmailPassword = "your-email-password";
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;

            MailMessage msg = new MailMessage();
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.HeadersEncoding = System.Text.Encoding.UTF8;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.From = new MailAddress(fromEmailAddress, fromEmailDisplayName);
            msg.To.Add("recipient-email@example.com");

            msg.Subject = "Email Subject";
            msg.Body = "Email Body";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                    | SecurityProtocolType.Tls11
                                    | SecurityProtocolType.Tls12;

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                client.EnableSsl = true;

                try
                {
                    client.Send(msg);
                    msg.Dispose();
                    return View("Success");
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    Console.WriteLine("Error sending email: " + ex.Message);
                    return View("Error");
                }
            }
        }

        
     }
}