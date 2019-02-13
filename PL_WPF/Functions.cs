using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BE;
using static BE.Configuration;

namespace PL_WPF
{
    internal static class Functions
    {
       public static MessageBoxResult ShowMessageToUser(Exception ex) 
            => MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

        public static bool SendMailToAdmin(Exception ex)
        {
            MessageBox.Show("There is a problem with the system that is not connected to you. \n A mail was sent to the developers.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show(ex.Message, "For inits", MessageBoxButton.OK, MessageBoxImage.Error);
            return true;
            MailMessage mail = new MailMessage(from: "?????@g.jct.ac.il", to: "??????@g.jct.ac.il")
            {
                Subject = "Error in mini project",
                Body = ex.ToString(),
                IsBodyHtml = true
            };
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Credentials = new System.Net.NetworkCredential("????????@gmail.com", "????????"),
                EnableSsl = true
            };
            try
            {
                smtp.Send(mail);
            }
            catch
            {
                return false;
                //Close();
            }

            return true;
        }


        internal static List<string> errorMessages = new List<string>();
        private static void Input_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                errorMessages.Add(e.Error.Exception.Message);
            else
                errorMessages.Remove(e.Error.Exception.Message);
        }

    }
    
}
