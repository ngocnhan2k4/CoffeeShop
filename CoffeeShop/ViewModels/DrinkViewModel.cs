using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels
{
    public class DrinkViewModel : INotifyPropertyChanged
    {
        public FullObservableCollection<Drink> Drinks
        {
            get; set;
        }
        public FullObservableCollection<Drink> ChosenDrinks { get; set; }
        public DrinkViewModel()
        {
            IDao dao = new MockDao();
            Drinks = new FullObservableCollection<Drink>(dao.GetDrinks());
            ChosenDrinks = new FullObservableCollection<Drink>();
        }
        public void AddOrRemoveDrink(Drink drink)
        {
            if (ChosenDrinks.Contains(drink))
            {
               // drink.IsChosen = false;
                ChosenDrinks.Remove(drink);
            }
            else
            {
               // drink.IsChosen = true;
                ChosenDrinks.Add(drink);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
