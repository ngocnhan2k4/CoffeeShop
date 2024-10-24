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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
