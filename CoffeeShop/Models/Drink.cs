using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    /// <summary>
    /// This class represents a drink
    /// </summary>
    public class Drink : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double OriginalPrice { get; set; }
        public double SalePrice { get; set; }
        public int CategoryID { get; set; }
        public int Stock { get; set; }
        public string Ingredients { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
