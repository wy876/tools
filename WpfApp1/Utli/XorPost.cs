using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace WpfApp1.Utli
{
    /// <summary>
    /// xor加密 http post请求类
    /// </summary>
    public class XorPost
    {
        public static string Post(string url, string jsonData)
        {
            string responseContent = string.Empty;

            // 代理类配置
            string Host = PrxoyChains();

            ProxyData rt = JsonConvert.DeserializeObject<ProxyData>(Host);

            string Purl = string.Format("{0}://{1}:{2}", rt.Proxy_Agreement, rt.Proxy_Host, rt.Proxy_Prot);



            //proxy.Credentials = new NetworkCredential("username", "password"); // 如果需要身份验证

            // 创建 HttpClientHandler 并设置代理
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            // 创建代理配置
            if (rt.Proxy_Agreement == string.Empty)
            {
                httpClientHandler.Proxy = null;

            }
            else if (rt.Proxy_Agreement != string.Empty)
            {
                WebProxy proxy = new WebProxy(Purl);
                httpClientHandler.Proxy = proxy;
            }
            httpClientHandler.UseProxy = true;

            using (HttpClient client = new HttpClient(httpClientHandler))
            {



                // Console.WriteLine(jsonData);

                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(url, content).Result;



                if (response.IsSuccessStatusCode)
                {
                    responseContent = response.Content.ReadAsStringAsync().Result;
                    //Console.WriteLine("Response: " + responseContent);

                }
                else
                {
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                }
            }
            return responseContent;

        }
        /// <summary>
        /// 代理类，从文件中读取代理Ip 和 端口
        /// </summary>
        /// <returns></returns>
        public static string PrxoyChains()
        {
            string res = string.Empty;
            try
            {
                //循环读取文件//一行一行的读取
                string[] contents = File.ReadAllLines(@"Data/proxy.txt", Encoding.Default);

                foreach (string item in contents)
                {

                    res = item;

                }
            }
            catch
            {
                MessageBox.Show("Proxy File Error");
            }

            return res;
        }


    }
}
