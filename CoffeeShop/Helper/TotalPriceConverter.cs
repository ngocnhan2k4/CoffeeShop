﻿using CoffeeShop.Models;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Helper
{
    public class TotalPriceConverter : IValueConverter
    {
        private readonly IntToVnCurrencyNoDConverter _currencyConverter = new IntToVnCurrencyNoDConverter();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DetailInvoice detailInvoice)
            {
                double totalPrice = detailInvoice.Quantity * detailInvoice.Price;
                return _currencyConverter.Convert(totalPrice, targetType, parameter, language);
            }
            return _currencyConverter.Convert(0, targetType, parameter, language);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
