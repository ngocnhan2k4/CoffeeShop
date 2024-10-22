using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels.Settings
{
    public class ProductsManagementViewModel: INotifyPropertyChanged
    {
        public List<Drink> Drinks { get; set; }

        public ProductsManagementViewModel()
        {
            Drinks =
            [
                new () { Name = "Add New Drink", ImageUrl = "", Price = 0.0, Quantity = 0 },
                new() { Name = "Cafe", ImageUrl = "Assets/download.jpg", Price = 2.29, Quantity = 20 },
                new() { Name = "Soda", ImageUrl = "Assets/download.jpg", Price = 2.69, Quantity = 30 },
                new() { Name = "Cafe", ImageUrl = "Assets/download.jpg", Price = 2.69, Quantity = 30 },
                new() { Name = "Soda", ImageUrl = "Assets/download.jpg", Price = 2.69, Quantity = 30 },
            ];
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
