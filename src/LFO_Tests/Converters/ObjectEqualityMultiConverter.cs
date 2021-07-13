using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFO_Tests.Converters
{
    public class ObjectEqualityMultiConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is { } && values.Count == 2 && values[0] != AvaloniaProperty.UnsetValue && values[1] != AvaloniaProperty.UnsetValue)
            {
                var result = values[0]?.Equals(values[1]);
                var r2 = values[0]?.ToString().Equals(values[1]);


                if (result.Value || r2.Value)
                    return true;
                return false;
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
