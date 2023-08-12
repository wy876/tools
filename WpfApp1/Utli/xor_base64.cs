using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.Utli
{
    /// <summary>
    /// xor+base64加解密类 
    /// </summary>
    public class xor_base64
    {
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encrypt(string data)
        {
            string key = "e45e329feb5d925b";
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                char encryptedChar = (char)(data[i] ^ key[(i + 1) & 15]);
                result.Append(encryptedChar);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(result.ToString());
            string base64Encoded = Convert.ToBase64String(bytes);

            return base64Encoded;
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Decrypt(string data)
        {
            string key = "e45e329feb5d925b";
            byte[] bytes = Convert.FromBase64String(data);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                char decryptedChar = (char)(bytes[i] ^ key[(i + 1) & 15]);
                result.Append(decryptedChar);
            }

            return result.ToString();


        }
    }
}
