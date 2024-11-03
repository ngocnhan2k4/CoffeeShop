using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    /// <summary>
    /// This class represents an invoice
    /// </summary>
    public class Invoice : INotifyPropertyChanged
    {
        public int InvoiceID { get; set; }
        public string CreatedAt { get; set; }
        public int TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string HasDelivery { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
