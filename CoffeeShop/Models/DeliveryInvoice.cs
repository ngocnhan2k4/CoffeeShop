using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    /// <summary>
    /// This class represents a delivery invoice
    /// </summary>
    public class DeliveryInvoice : INotifyPropertyChanged
    {
        public int DeliveryInvoiceID {get; set;}
        public string PhoneNumber { get; set; }
        public int ShippingFee { get; set; }
        public string Address { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
