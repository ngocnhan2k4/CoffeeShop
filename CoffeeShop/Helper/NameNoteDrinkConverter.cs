using CoffeeShop.Models;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Helper
{
    public class NameNoteDrinkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DetailInvoice detailInvoice)
            {
                var nameDrink = detailInvoice.NameDrink;
                var note = detailInvoice.Note;

                if (string.IsNullOrEmpty(note))
                {
                    return nameDrink;
                }
                return $"{nameDrink} - ({note})";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    
}
