using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public static class NimboleCommon
    {
        public static void SetAlert(this Controller controller, AlertMessageViewModel alert)
        {
            controller.TempData["Alert"] = alert;
        }

        public static bool SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.sendgrid.net", 587))
                {
                    System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage(from, to, subject, body);

                    client.UseDefaultCredentials = false;
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_a8584cff310026242274ca6ce504cc2d@azure.com", "499JfDVQNC2PV0y");
                    client.Credentials = credentials;
                    client.Send(email);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public static bool IsNumeric(string parameter, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(parameter, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        public static bool IsAuthorized(string controller, List<string> modules)
        {
            if (modules.Contains(controller))
                return true;
            else
                return false;
        }
    }

    public class ClsCrypto
    {
        private RijndaelManaged myRijndael = new RijndaelManaged();
        private int iterations;
        private byte[] salt;

        public ClsCrypto(string strPassword)
        {
            //myRijndael.BlockSize = 128;
            myRijndael.KeySize = 128;
            myRijndael.IV = Encoding.UTF8.GetBytes ("8080808080808080");

            myRijndael.Padding = PaddingMode.PKCS7;
            myRijndael.Mode = CipherMode.CBC;
            //iterations = 1000;
            salt = System.Text.Encoding.UTF8.GetBytes("8080808080808080");
            //myRijndael.Key = GenerateKey(strPassword);
            myRijndael.Key = Encoding.UTF8.GetBytes("8080808080808080");
        }

        public string Encrypt(string strPlainText)
        {
            byte[] strText = new System.Text.UTF8Encoding().GetBytes(strPlainText);
            ICryptoTransform transform = myRijndael.CreateEncryptor();
            byte[] cipherText = transform.TransformFinalBlock(strText, 0, strText.Length);
            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            ICryptoTransform transform = myRijndael.CreateDecryptor();
            byte[] cipherText = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return System.Text.Encoding.UTF8.GetString(cipherText);
        }

        public static byte[] HexStringToByteArray(string strHex)
        {
            dynamic r = new byte[strHex.Length / 2];
            for (int i = 0; i <= strHex.Length - 1; i += 2)
            {
                var substr = strHex.Substring(i, 2);
                r[i / 2] = Convert.ToByte(Convert.ToInt32(substr, 16));
            }
            return r;
        }

        private byte[] GenerateKey(string strPassword)
        {
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(System.Text.Encoding.UTF8.GetBytes(strPassword), salt, iterations);
            return rfc2898.GetBytes(128 / 8);
        }
    }
}