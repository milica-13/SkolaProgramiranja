using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System;
using System.IO;
using System.Windows.Data;

namespace SkolaProgramiranja.Models
{
    public class RelativePathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;

            if (string.IsNullOrEmpty(path))
                return null;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slike", path);

            if (!File.Exists(fullPath))
                return null;

            return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
