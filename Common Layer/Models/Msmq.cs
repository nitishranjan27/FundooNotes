using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Common_Layer.Models
{
    public class Msmq
    {
        MessageQueue messageQueue = new MessageQueue();
        public void SendMessage(string token)
        {
            messageQueue.Path = @".\private$\Token";//for windows path

            if (!MessageQueue.Exists(messageQueue.Path))
            {

                MessageQueue.Create(messageQueue.Path);

            }
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
            messageQueue.Send(token);
            messageQueue.BeginReceive();
            messageQueue.Close();
        }

        public void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var message = messageQueue.EndReceive(e.AsyncResult);
            string token = message.Body.ToString();
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "Fundoo Notes Reset Link";
            mailMessage.IsBodyHtml = true;
            //string bodymessage = @"<p>Your password has been reset.Please click the link to create new password.</p>" + string.Format("<a href=\"https://localhost:4200/api/User/ResetPassword.aspx?token={0}\">Reset Password Link</a>");
            //mailMessage.Body = "Copy the token Below to reset your password :  \n\n" + token;
            mailMessage.Body = $"<!DOCTYPE html>" +
                                   $"<html>" +
                                   $"<html lang=\"en\">" +
                                    $"<head>" +
                                    $"<meta charset=\"UTF - 8\">" +
                                    $"</head>" +
                                    $"<body>" +
                                    $"<h1> Click The Below Link To Reset Your Password</h1>" +
                                    $"<a href='http://localhost:4200/reset/{token}'> ClickHere </a>\n " +
                                    $"</body>" +
                                   $"</html>";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("nitishrkranjan@gmail.com", "N_itish8250"),
            };
            mailMessage.From = new MailAddress("nitishrkranjan@gmail.com");
            mailMessage.To.Add(("nitishrkranjan@gmail.com"));
            smtpClient.Send(mailMessage);
        }
    }
}

