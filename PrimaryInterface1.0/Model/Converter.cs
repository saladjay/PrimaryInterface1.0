using ExtendedString;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PrimaryInterface1._0.Model
{
    public sealed class Converter
    {
        public static StateConverter CellStateConverter
        {
            get { return SingleTon<StateConverter>.GetInstance(); }
        }
    }

    public sealed class StateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)values[0] && (bool)values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
