using CoffeeShop.Helper;
using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels.HomePage
{
    public class ChoseDrinkViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public FullObservableCollection<DetailInvoice> ChosenDrinks { get; set; }
        public decimal TotalPrice { get; set; }

        public ChoseDrinkViewModel()
        {
            ChosenDrinks = new FullObservableCollection<DetailInvoice>();
        }
        public void AddDrink(Drink drink, Size size)
        {
            var existingInvoice = ChosenDrinks.FirstOrDefault(di => di.NameDrink == drink.Name && di.Size == size.Name);
            if (existingInvoice != null)
            {
                existingInvoice.Quantity += 1;
            }
            else
            {
                var newInvoice = new DetailInvoice
                {
                    NameDrink = drink.Name,
                    Quantity = 1,
                    Size = size.Name,
                    Price = size.Price
                };
                ChosenDrinks.Add(newInvoice);
            }
            TotalPrice = ChosenDrinks.Sum(di => di.Price * di.Quantity);
        }
        public void RemoveDrink(DetailInvoice detail)
        {

            ChosenDrinks.Remove(detail);
            TotalPrice = ChosenDrinks.Sum(di => di.Price * di.Quantity);
        }
    }
}
