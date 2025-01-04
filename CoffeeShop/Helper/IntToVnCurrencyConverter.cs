using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace CoffeeShop.Helper
{
    /// <summary>
    /// This class is used to convert a double value to a string with Vietnamese currency format.
    /// </summary>
    public class IntToVnCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double)
            {
                return ((double)value).ToString("C0", new CultureInfo("vi-VN"));
            }
            else if(value is int)
            {
                return ((int)value).ToString("C0", new CultureInfo("vi-VN"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}