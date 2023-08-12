using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Utli;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace WpfApp1.UI
{
    /// <summary>
    /// Proxy.xaml 的交互逻辑
    /// </summary>
    public partial class Proxy : Window
    {
        public static ObservableCollection<ProxyData> ProxyData = new ObservableCollection<ProxyData>(); //及时更新


        public Proxy()
        {
            InitializeComponent();
            IsFile();
        }
      
        
        /// <summary>
        ///  RadioButton 1 禁止设置代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void box1_Click(object sender, RoutedEventArgs e)
        {
            if (box1.IsChecked == true)
            {
                
                text1.IsEnabled = false;
                text2.IsEnabled = false;
                Button.IsEnabled = false;
                combox1.IsEnabled = false;
                text1.Clear();
                text2.Clear();
                try
                {
                    string Fopen = String.Format("{{\"Proxy_Agreement\": \"{0}\",\"Proxy_Host\": \"{1}\",\"Proxy_Prot\": \"{2}\"}}", combox1.Text, text1.Text, text2.Text);

                    await File.WriteAllTextAsync(@"Data/proxy.txt", Fopen);
                    MessageBox.Show("已取消代理！");
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// RadioButton 2 手动设置代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

       
        private void box2_Click(object sender, RoutedEventArgs e)
        {
            
            if (box2.IsChecked == true)
            {
                text1.IsEnabled = true;
                text2.IsEnabled = true;
                Button.IsEnabled = true;
                combox1.IsEnabled = true;

            }

        }
        

        /// <summary>
        /// 代理 保存按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            

            if ((String.IsNullOrEmpty(text1.Text)))
            {


                MessageBox.Show("Ip不能为空");
            }
            else if (String.IsNullOrEmpty(text2.Text))
            {
                MessageBox.Show("端口不能为空！");
            }
            else if (String.IsNullOrEmpty(combox1.Text))
            {
                MessageBox.Show("协议不能为空！");
            }
            else
            {
                
                string Fopen = String.Format("{{\"Proxy_Agreement\": \"{0}\",\"Proxy_Host\": \"{1}\",\"Proxy_Prot\": \"{2}\"}}", combox1.Text, text1.Text, text2.Text);

                await File.WriteAllTextAsync(@"Data/proxy.txt", Fopen);
                MessageBox.Show("保存成功");
                
               
            }

        }

       public void IsFile()
        {
            try
            {
                //循环读取文件//一行一行的读取
                string[] contents = File.ReadAllLines(@"Data/proxy.txt", Encoding.Default);

                foreach (string item in contents)
                {
                    //解析json数据 
                    ProxyData rt = JsonConvert.DeserializeObject<ProxyData>(item);

                    if(rt.Proxy_Agreement != String.Empty)
                    {
                        take_effect.Content = "代理生效";
                      
                    }
                    else
                    {
                        take_effect.Content = "";
                    }
                }
            }
            catch
            {

            }
        }

       

      
    }
}
