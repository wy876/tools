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

namespace WpfApp1.UI
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string s = "免责声明：工具本身并无好坏，希望大家以遵守《网络安全法》相关法律为前提来使用该工具，支持研究学习，切勿用于非法犯罪活动，对于恶意使用该工具造成的损失，和本人及开发者无关。";
            textbox.Text = s;
        }
    }
}
