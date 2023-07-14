using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpfApp1.UI;


/*
使用例子
  string rsults = Post.Postring("url",new Dictionary<string,string>{{"username":"admin"}, {"password":"123456"},})
*/


namespace WpfApp1.Utli
{
    /// <summary>
    /// POst 请求类
    /// </summary>
    public class Post 
    {


        
        /// <summary>
        /// post请求
        /// </summary>
        /// <returns></returns>
        public static string Postring(string url,Dictionary<string,string> dic)
        {
            string result = "";

            try
            {
                string Host = PrxoyChains();

                ProxyData rt = JsonConvert.DeserializeObject<ProxyData>(Host);

                string Purl = string.Format("{0}://{1}:{2}", rt.Proxy_Agreement, rt.Proxy_Host, rt.Proxy_Prot);

                
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.Headers.Add("user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
                req.ContentType = "application/x-www-form-urlencoded";
                //req.Proxy = null;
            
                
               

                if (rt.Proxy_Agreement == string.Empty)
                {
                    req.Proxy = null;

                }
                else if (rt.Proxy_Agreement != string.Empty)
                {

                    // 设置代理
                    WebProxy proxy = new WebProxy(Purl);
                    req.Proxy = proxy;

                }




                req.KeepAlive = false;

                //添加post 参数
                StringBuilder builder = new StringBuilder();
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
                byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                
                Stream stream = response.GetResponseStream();

                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("webshell连接超时！");
                result = "webshell连接超时";


            }

            
            return result;
            
        }

        
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
