﻿using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoffeeShop.Service.DataAccess
{
    public interface IDao
    {
        public enum SortType
        {
            Ascending,
            Descending
        }
        List<Drink> GetDrinks();
        public Tuple<List<Drink>, int> GetDrinks(
            int page, int rowsPerPage,
            string keyword, int categoryID,
            Dictionary<string, SortType> sortOptions
        );
        List<Category> GetCategories();
        List<DeliveryInvoice> GetDeliveryInvoices();
        List<Invoice> GetInvoices();
        List<DetailInvoice> GetDetailInvoices();
    }
}