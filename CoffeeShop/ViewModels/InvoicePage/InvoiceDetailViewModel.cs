using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels.InvoicePage
{
    public class InvoiceDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public FullObservableCollection<DetailInvoice> detailInvoices { get; set; }
        IDao _dao;
        public Invoice _invoice { get; set; }
        public DeliveryInvoice deliveryInvoice { get; set; }
        public Visibility ButtonVisibility => _invoice?.Status == "Wait" ? Visibility.Visible : Visibility.Collapsed;

        public InvoiceDetailViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            detailInvoices = new FullObservableCollection<DetailInvoice>();
        }
        public void SetDetailInvoices(Invoice invoice)
        {
            _invoice = invoice;
            var (invoiceDetails,deliveryInvoice ) = _dao.GetDetailInvoicesOfId(invoice.InvoiceID);
            detailInvoices = new FullObservableCollection<DetailInvoice>(invoiceDetails);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void setStatus(string status)
        {
            _invoice.Status = status;
            OnPropertyChanged("ButtonVisibility");
            _dao.UpdateInvoiceStatus(_invoice.InvoiceID, status);
        }
    }
}
