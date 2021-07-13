using Avalonia.Data.Converters;
using LFO_Tests.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFO_Tests.Converters
{
    public class ControlValueConverter : IValueConverter
    {   

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
