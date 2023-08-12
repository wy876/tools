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
    /// item.xaml 的交互逻辑
    /// </summary>
    public partial class item : Window
    {
        public item()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //向主窗口传值 
            MessageBox.Show("我是确认按钮");

        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void ShowDialog(MainWindow mainWindow)
        {
            throw new NotImplementedException();
        }
    }
}
