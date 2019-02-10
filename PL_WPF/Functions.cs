using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BE;
using static BE.Configuration;

namespace PL_WPF
{
    internal static class Functions
    {
       public static MessageBoxResult ShowMessageToUser(Exception ex) 
            => MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);

        public static bool SendMailToAdmin(Exception ex)
        {
            return false;
            MailMessage mail = new MailMessage(from: "goldis@g.jct.ac.il", to: "goldis@g.jct.ac.il")
            {
                Subject = "Error in mini project",
                Body = ex.ToString(),
                IsBodyHtml = true
            };
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Credentials = new System.Net.NetworkCredential("rafig0601@gmail.com", "rrrrrr"),
                EnableSsl = true
            };
            try
            {
                smtp.Send(mail);
            }
            catch
            {
                //Close();
            }

            return true;
        }
    }
    
}
