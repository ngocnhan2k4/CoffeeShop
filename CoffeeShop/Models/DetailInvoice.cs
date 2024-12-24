using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    /// <summary>
    /// This class represents a detail invoice
    /// </summary>
    public class DetailInvoice : INotifyPropertyChanged
    {
        public int InvoiceID { get; set; }
        public int DrinkId { get; set; }
        public string NameDrink { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public int MaxQuantity { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
