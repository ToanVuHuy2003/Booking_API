using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace FlutterCinemaAPI.Models.Utils
{
    public class GoogleSMTP
    {
        // Đọc tài khoản và mật khẩu từ web.config | app.config
        private static readonly string
             USER_EMAIL = ConfigurationManager.AppSettings["sender_email"],
             USER_PASSWORD = ConfigurationManager.AppSettings["sender_password"];
        // Tiêu đề email
        private static readonly string
             EMAIL_SUBJECT = "Đặt lại mật khẩu ứng dụng";
        // Địa chỉ máy chủ & port của Google SMTP
        private static readonly string
             SMTP_SERVER = "smtp.gmail.com";
        private static readonly short
             SMTP_PORT = 587;

        public static bool SendEmail(string receiverEmail, string emailBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(GoogleSMTP.SMTP_SERVER);

                mail.From = new MailAddress(GoogleSMTP.USER_EMAIL);
                mail.To.Add(receiverEmail);
                // Tiêu đề email
                mail.Subject = GoogleSMTP.EMAIL_SUBJECT;
                // Nội dung Email
                mail.Body = emailBody;
                // Priority càng cao thì xác suất vào Spam folder càng thấp
                mail.Priority = MailPriority.High;

                smtpServer.Port = GoogleSMTP.SMTP_PORT;
                smtpServer.Credentials = new NetworkCredential(
                    GoogleSMTP.USER_EMAIL,
                    GoogleSMTP.USER_PASSWORD
                );
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }
    }
}