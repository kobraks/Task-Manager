using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Task_Manager
{
    public class Config
    {
        ushort _port = 2555;
        string _password = "password";
        string _hasedPassword = "";

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                _hasedPassword = GetMD5Hash(_password);
            }
        }

        public ushort Port
        {
            get
            {
                return _port;
            }

            set
            {
                _port = value;
            }
        }

        public static String GetMD5Hash(String TextToHash)
        {
            //Check wether data was passed
            if ((TextToHash == null) || (TextToHash.Length == 0))
            {
                return String.Empty;
            }

            //Calculate MD5 hash. This requires that the string is splitted into a byte[].
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(TextToHash);
            byte[] result = md5.ComputeHash(textToHash);

            //Convert result back to string.
            return System.BitConverter.ToString(result);
        }

        public Config()
        {
            Password = "Password";
        }

        public void Load()
        {

        }

        public void Save()
        {

        }
    }
}
