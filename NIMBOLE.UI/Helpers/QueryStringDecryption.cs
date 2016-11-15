using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using NIMBOLE.UI.Controllers;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Web.Routing;



namespace NIMBOLE.UI.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {

                Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
                if (HttpContext.Current.Request.QueryString.Get("q") != null)
                {
                    string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q").Replace(' ', '+');
                    //string decrptedString = Decrypt(encryptedQueryString.ToString());

                   
                        string decryptedString = DecryptStringAES(encryptedQueryString.ToString());

                        if (decryptedString.Contains("?"))
                        {
                            string[] paramsArrs = decryptedString.Split('?');

                            for (int i = 0; i < paramsArrs.Length; i++)
                            {
                                string[] paramArr = paramsArrs[i].Split('=');
                                object value = paramArr[1];
                                if (paramArr[0].ToString() == "Id")
                                    decryptedParameters.Add(paramArr[0], Convert.ToInt32(value));
                                else
                                    decryptedParameters.Add(paramArr[0], value);
                            }
                        }
                        else
                        {
                            if (NimboleCommon.IsNumeric(decryptedString, System.Globalization.NumberStyles.Integer))
                                decryptedParameters.Add("Id", Convert.ToInt32(decryptedString));
                            else
                            {
                                string[] paramArr = decryptedString.Split('=');
                                object value = paramArr[1];
                                if (paramArr[0].ToString() == "Id")
                                    decryptedParameters.Add(paramArr[0], Convert.ToInt32(value));
                                else
                                    decryptedParameters.Add(paramArr[0], value);
                            }
                        }                   
                }

               
                    for (int i = 0; i < decryptedParameters.Count; i++)
                    {
                        filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
                    }
               
                base.OnActionExecuting(filterContext);
            }
            catch (Exception ex)
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null ||
                      filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                {
                    char[] delimiterChars = { '/' };
                    string strfilter = string.Empty;
                    strfilter = filterContext.HttpContext.Request.FilePath.ToString();
                    string[] words = strfilter.Split(delimiterChars);
                    string strController = string.Empty;
                    strController = words[1].Trim();
                    if (!string.IsNullOrEmpty(strController) && strController == "Contacts")
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                                       RouteValueDictionary(new { controller = strController, action = "ListviewIndex", area = "" }));
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                                       RouteValueDictionary(new { controller = strController, action = "Index", area = "" }));
                    }
                }
                base.OnActionExecuting(filterContext);
                //throw ex;
               // throw new InvalidOperationException(ex.Message);
            }
        }

        public static string DecryptStringAES(string cipherText)
        {
            try
            {

                var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
                var iv = Encoding.UTF8.GetBytes("8080808080808080");
                //if (cipherText.Length <= 22)
                //{
                //    return RedirectToAction("Index");
                //}
                //cipherText.Trim().Replace(" ", "+");
                //cipherText = cipherText.Substring(0, 22);
                //if (cipherText.Length >= 22)
                //{
                //    if (cipherText.Length % 4 > 0)
                //        cipherText = cipherText.PadRight(cipherText.Length + 4 - cipherText.Length % 4, '=');
                //    // return Encoding.UTF8.GetString(Convert.FromBase64String(s));

                //    var encrypted = Convert.FromBase64String(cipherText);
                     var encrypted = Convert.FromBase64String(cipherText);
                    var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                    return string.Format(decriptedFromJavascript);
                //}
                //else
                //{
                //    return string.Empty;
                //}
            }
            catch (Exception ex) 
            {
                throw ex;
               // throw new InvalidOperationException(ex.Message);
            }
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
                
      

        //private string Decrypt(string encryptedText)
        //{
        //    string key = "jdsg432387#";
        //    byte[] DecryptKey = { };
        //    byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
        //    byte[] inputByte = new byte[encryptedText.Length];

        //    DecryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    inputByte = Convert.FromBase64String(encryptedText);
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
        //    cs.Write(inputByte, 0, inputByte.Length);
        //    cs.FlushFinalBlock();
        //    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        //    return encoding.GetString(ms.ToArray());
        //}
    }
}