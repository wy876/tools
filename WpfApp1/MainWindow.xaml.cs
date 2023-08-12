using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using WpfApp1.UI;
using WpfApp1.Utli;
using System.IO;
using WpfApp1.UI.aspx;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Net;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static ObservableCollection<ListData> ListData = new ObservableCollection<ListData>(); //及时更新

        

        public MainWindow()
        {
            InitializeComponent();
            ListView.ItemsSource = ListData;

        }

        public string Ip;
        public string address;
        /// <summary>
        /// 添加数据 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Addshell  add = new Addshell();
            add.Owner = this;
            add.Show();
        }

     
        /// <summary>
        /// 代理设置 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Proxy proxy = new Proxy();
            proxy.Show();
        }

        /// <summary>
        /// 编辑数据 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
         
            //获取listview 数据
            ListData listData = ListView.SelectedItem as ListData;
            if(listData != null && listData is ListData)
            {
                EditShell edit = new EditShell();
                edit.getUrl = listData.url;
                edit.getPwd = listData.Password;
                edit.getScript = listData.Script;
                edit.getEncode = listData.Encode;
                edit.getDef = listData.defaults;
                edit._ID = listData.ID;
                edit.Show();
            }
            
                
           
        }

        /// <summary>
        /// 初始化 配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           
            //循环读取文件//一行一行的读取
            string[] contents = File.ReadAllLines(@"Data/ant.txt", Encoding.Default);

            //获取文件时间
            DateTime fileCreatedDate = File.GetCreationTime(@"Data/ant.txt");

            //定义数组 把defaults 数据 添加到数组中
            List<string> listS = new List<string>();

            foreach (string item in contents) {
                //解析json数据 
                JsData rt = JsonConvert.DeserializeObject<JsData>(item);

                string[] urls = rt.url.ToString().Split("/");
                string U = urls[2];
                Regex isYuming = new Regex("[a-zA-Z]");
                bool b = isYuming.IsMatch(U);
                if (b)
                {
                    string strIP;
                    if (GetYuMingIP(U, out strIP))
                    {
                        Ip = strIP;
                        address = GetIP(strIP);
                    }


                }
                else
                {
                    Ip = U;
                    address = GetIP(U);
                }

                if (!listS.Contains(rt.defaults))
                    listS.Add(rt.defaults);

                if (rt.defaults == "Initial")
                {
                    ListData.Add(new ListData() {ID=rt.ID,url=rt.url, Password = rt.Password, Script = rt.Script, defaults = rt.defaults, Ip=Ip,address = address, Encode = rt.Encode });
                }

            }

            //List用于存储从数组里取出来的不相同的元素
            List<string> listString = new List<string>();

            foreach (string eachString in listS)
            {
                if (!listString.Contains(eachString))
                    listString.Add(eachString);
            }

            //最后从List里取出各个字符串进行操作
            foreach (string eachString in listString)
            {
                // TreeVie 添加数据
                var item = new TreeViewItem()
                {
                    Header = eachString,

                };
                TreeView.Items.Add(item);
            }

        }

        /// <summary>
        /// TreeViewItem 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (TreeView.SelectedItem != null)
            {
                ListData.Clear();
                //获取节点的 值
                TreeViewItem treeViewItem = (TreeViewItem)TreeView.SelectedItem;
                var model = treeViewItem.Header;

                string[] contents = File.ReadAllLines(@"Data/ant.txt", Encoding.Default);

                foreach (string def in contents)
                {
                    //解析json数据 
                    JsData rt = JsonConvert.DeserializeObject<JsData>(def);
                    string[] urls = rt.url.ToString().Split("/");
                    string U = urls[2];
                    Regex isYuming = new Regex("[a-zA-Z]");
                    bool b = isYuming.IsMatch(U);
                    if (b)
                    {
                        string strIP;
                        if (GetYuMingIP(U, out strIP))
                        {
                            Ip = strIP;
                            address = GetIP(strIP);
                        }       
                    }
                    else
                    {
                        Ip = U;
                        address = GetIP(U);
                    }

                    if (rt.defaults == model.ToString())
                    {
                        ListData.Add(new ListData {ID=rt.ID ,url = rt.url, Password = rt.Password, Script = rt.Script,Ip=Ip,defaults = rt.defaults,address = address, Encode = rt.Encode });
                       
                    }

                }
               

            }

            
        }

        /// <summary>
        ///  ListView 删除数据 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            int Index=0;
            ListData listData = ListView.SelectedItem as ListData;
            if (listData != null && listData is ListData)
            {
                
            }

        }
        /// <summary>
        /// 文件管理 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取listview 数据
                ListData listData = ListView.SelectedItem as ListData;
                if (listData != null && listData is ListData)
                {
                    if (listData.Script == "php")
                    {
                        FileList fileList = new FileList();
                        fileList.getUrl = listData.url;
                        fileList.getPwd = listData.Password;
                        fileList.Script = listData.Script;
                        fileList.Show();
                    }
                    if (listData.Script == "aspx")
                    {
                        AFileTree AFile = new AFileTree();
                        AFile.Password = listData.Password;
                        AFile.shell = listData.url;
                        AFile.Script = listData.Script;
                        AFile.Show();
                    }



                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            


        }

        /// <summary>
        /// 右键点击 选中 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           if(ListView.SelectedItem != null)
            {
                Virtual_terminals.IsEnabled = true;
                File_management.IsEnabled = true;
                Edit_data.IsEnabled = true;
                del_data.IsEnabled = true;
            }
            else
            {
                Virtual_terminals.IsEnabled = false;
                File_management.IsEnabled = false;
                Edit_data.IsEnabled = false;
                del_data.IsEnabled=false;
            }
        }
        /// <summary>
        /// 虚拟终端 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Virtual_terminals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取listview 数据
                ListData listData = ListView.SelectedItem as ListData;
                if (listData != null && listData is ListData)
                {
                    if (listData.Script == "php")
                    {
                        terminals terminals = new terminals();
                        terminals.getUrl = listData.url;
                        terminals.getPwd = listData.Password;
                        terminals.Show();
                        /*  测试界面
                        Window1 Window1 = new Window1();
                        Window1.getUrl = listData.url;
                        Window1.getPwd = listData.Password;
                        Window1.Show();*/
                    }
                    if (listData.Script == "aspx")
                    {
                       
                        ATrminals aTrminals = new ATrminals();
                        aTrminals.getUrl = listData.url;
                        aTrminals.getPwd = listData.Password;
                        aTrminals.Show();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
          
        }
        /// <summary>
        /// 关于窗口 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        /// <summary>
        /// 数据库操作 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DB_operate_Click(object sender, RoutedEventArgs e)
        {
            ListData listData = ListView.SelectedItem as ListData;
            if (listData != null && listData is ListData) 
            {
                db db = new db();
                db.getUrl = listData.url;
                db.getScipt = listData.Script;
                db.getPwd = listData.Password;
                db.Show();
            }

        }


        /// <summary>
        /// 域名转ip
        /// </summary>
        /// <param name="url"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        private bool GetYuMingIP(string url, out string IP)
        {
            string p = @"(http|https)://(?<domain>[^(:|/]*)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            string ipAddress = url;
            if (!ipAddress.Contains("http"))
            {
                ipAddress = "http://" + ipAddress;
            }

            Match m = reg.Match(ipAddress);
            string Result = m.Groups["domain"].Value;//域名地址   如http://wwww.luofenmng.com/index.aspx  提取出来的是www.luofenming.com

            //以下是获取域名解析的IP地址
            try
            {
                IPHostEntry host = Dns.GetHostEntry(Result);
                IPAddress ip = host.AddressList[0];
                IP = ip.ToString();
            }
            catch (Exception ex)
            {
                IP = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取Ip归属地
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetIP(string str)
        {

            IpQuery_QQWry _ip_query = new IpQuery_QQWry("Data/qqwry.dat");
            var (_info0, _info1, _desp) = _ip_query.find_info(str);
            string ips = $"{_info0}  {_desp}";

            return ips;
        }

        

    }
}

