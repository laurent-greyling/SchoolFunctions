using SchoolFunctions.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SchoolFunctions.Helpers
{
    public class EmailService
    {
        public void SendMail(StudentModel clientDetails)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(Environment.GetEnvironmentVariable("Email"));
                mail.To.Add(clientDetails.Email);
                mail.Subject = $"{clientDetails.Course} SignUp";

                var body = new StringBuilder();
                body.Append("<div>");
                body.Append("<div style='font-family: Calibri, sans-serif'></div>");
                body.Append($"<p>Hi {clientDetails.Name} {clientDetails.Surname}</p>");
                body.Append($"<p>We received a signup request for the cours: {clientDetails.Course}</p>");
                body.Append("<p>");
                body.Append($"<p>{clientDetails.Reason}</p>");
                body.Append("</p>");
                body.Append("<p><div>Regards</div><div>Your Freindly Management System</div></p>");
                body.Append("</div>");

                mail.Body = body.ToString();
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                Send(mail);
            }
        }

        private void Send(MailMessage mail)
        {
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) //587 or 465(ssl)
            {
                smtp.EnableSsl = true;
                smtp.TargetName = "STARTTLS/smtp.gmail.com";
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("Email"), Environment.GetEnvironmentVariable("EmailPsw"));
                smtp.Send(mail);
            }
        }
    }
}
