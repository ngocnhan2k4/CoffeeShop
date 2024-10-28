using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoffeeShop.Service.DataAccess
{
    public interface IDao
    {
        List<Drink> GetAllDrinks();
        List<Category> GetCategories();
    }
}