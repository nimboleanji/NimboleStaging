using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Common
{
    public static class Helper
    {
        public static string Encrypt(string password)
        {
            string encryptedPassword = string.Empty;
            try
            {
                byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(password);
                encryptedPassword = Convert.ToBase64String(b);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return encryptedPassword;
        }
        public static string Decrypt(string password)
        {
            string decryptedPassword = string.Empty;
            try
            {
                byte[] b = Convert.FromBase64String(password);
                decryptedPassword = System.Text.ASCIIEncoding.ASCII.GetString(b);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return decryptedPassword;
        }
        private static String _connectionStringName = "";
        public static String ConnectionStringName
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionStringName))
                {
                    _connectionStringName = "CRMConnectionString";
                }

                return _connectionStringName;
            }

            set
            {
                _connectionStringName = value;
            }
        }
        public static string CreateRandomPassword(int PasswordLength)
        {
            string employeeGeneratePassword = "NoPassword";
            try
            {
                string allowedChars = "";
                employeeGeneratePassword = "";
                allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
                allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
                allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,$";
                char[] sep = { ',' };
                string[] arr = allowedChars.Split(sep);
                string temp = "";
                Random rand = new Random();

                for (int i = 0; i < PasswordLength; i++)
                {
                    temp = arr[rand.Next(0, arr.Length)];
                    employeeGeneratePassword += temp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeGeneratePassword;
        }
    }
}
