using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NIMBOLE.Common
{
    public static class EmailSender
    {
        public static void SendEmailToNewUser(string subject, string emailBody, string email)
        {
            
            string smtpServer = string.Empty;
            string smtpPort = string.Empty;
            string smtpUserName = string.Empty;
            string smtpPassword = string.Empty;


            //smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
            //smtpPort = ConfigurationManager.AppSettings["SmtpPort"].ToString();
            //smtpUserName = ConfigurationManager.AppSettings["Username"].ToString();
            //smtpPassword = ConfigurationManager.AppSettings["Password"].ToString();
            smtpServer = "smtp.sendgrid.net";
            smtpPort = "25";
            smtpUserName = "nimbole";
            smtpPassword = "Nimbole123!";
            if (emailBody.Contains("\r\n"))
                emailBody = emailBody.Replace(System.Environment.NewLine, "<br/>");

            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpServer))
            {

                System.Net.Mail.MailMessage objeto_mail = new System.Net.Mail.MailMessage();
                //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                client.Port = Convert.ToInt32(smtpPort);

                client.Host = smtpServer;
                client.Timeout = 10000;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
                objeto_mail.From = new System.Net.Mail.MailAddress("admin@nimbole.com", "Nimbole Technologies", System.Text.Encoding.UTF8);
                objeto_mail.To.Add(new System.Net.Mail.MailAddress(email));
                objeto_mail.Subject = subject;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = emailBody;
                client.Send(objeto_mail);
            }
        }













        //public static void SendEmailToMe(string subject, string emailBody, string email)
        //{

        //    string smtpServer = string.Empty;
        //    string smtpPort = string.Empty;
        //    string smtpUserName = string.Empty;
        //    string smtpPassword = string.Empty;


        //    smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
        //    smtpPort = ConfigurationManager.AppSettings["SmtpPort"].ToString();
        //    smtpUserName = ConfigurationManager.AppSettings["TempEmail"].ToString();
        //    smtpPassword = ConfigurationManager.AppSettings["TempPwd"].ToString();


        //    if (emailBody.Contains("\r\n"))
        //        emailBody = emailBody.Replace(System.Environment.NewLine, "<br/>");

        //    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpServer))
        //    {

        //        System.Net.Mail.MailMessage objeto_mail = new System.Net.Mail.MailMessage();
        //        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

        //        client.Port = Convert.ToInt32(smtpPort);

        //        client.Host = smtpServer;
        //        client.Timeout = 10000;
        //        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;

        //        client.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
        //        objeto_mail.From = new System.Net.Mail.MailAddress("sreedhar.konam@xpio.com", "Intercity Transit BCC", System.Text.Encoding.UTF8);
        //        objeto_mail.To.Add(new System.Net.Mail.MailAddress(email));
        //        objeto_mail.Subject = subject;
        //        objeto_mail.IsBodyHtml = true;
        //        objeto_mail.Body = emailBody;
        //        client.Send(objeto_mail);
        //    }
        //}
    }
}