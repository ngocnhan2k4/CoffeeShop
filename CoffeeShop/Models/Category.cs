using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    /// <summary>
    /// This class represents a category of products
    /// </summary>
    public class Category : INotifyPropertyChanged
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public Category()
        {
            CategoryID = 0;
            CategoryName = "";
        }

        public Category(Category other)
        {
            CategoryID = other.CategoryID;
            CategoryName = other.CategoryName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
