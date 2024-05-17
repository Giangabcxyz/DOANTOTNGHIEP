using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebsiteChungKhoann.Controllers
{
    public class ContactController : Controller
    {
       
        // GET: Contact
        public ActionResult Send()
        {
            var fromEmailAddress = "Địa chỉ Email gửi";
            var fromEmailDisplayName = "Tên hiển thị của Email";
            var fromEmailPassword = "Mật khẩu được cấp khi tạo cái để gửi email";
            var smtpHost = "smtp.gmail.com";
            var smtpPort = "587";


            MailMessage msg = new MailMessage();
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.HeadersEncoding = System.Text.Encoding.UTF8;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.From = new MailAddress(fromEmailAddress, fromEmailDisplayName);
            msg.To.Add("Địa chỉ nhận Email");

            msg.Subject = "Tiêu đề Email";

            msg.IsBodyHtml = true;
            msg.Body = "Nội dung Email";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                    | SecurityProtocolType.Tls11
                                    | SecurityProtocolType.Tls12;

            var client = new SmtpClient();

            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = true;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.TargetName = "STARTTLS/smtp.office365.com";
            try
            {
                client.Send(msg);
                msg.Dispose();
                return View("Login");
            }
            catch
            {
                return View();
            }
        }
    }
}