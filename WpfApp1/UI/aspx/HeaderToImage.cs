using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfApp1.UI.aspx
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImage : IValueConverter
    {

        public static HeaderToImage Instance = new HeaderToImage();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the full path
            var path = (string)value;

            // If the path is null, ignore
            if (path == null)
                return null;

            // By default, we presume an image
            var image = "Images/file.png";

            if (path.Contains(":\\") == true)
                image = "Images/folder-closed.png";
            else
            {
                image = "Images/drive.png";
            }



            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
