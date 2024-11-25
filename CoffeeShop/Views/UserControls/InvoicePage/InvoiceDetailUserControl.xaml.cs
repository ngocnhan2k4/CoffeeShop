using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.ComponentModel;
using CoffeeShop.Helper;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.InvoicePage
{
    public sealed partial class InvoiceDetailUserControl : UserControl
    {
        public delegate void EventHandler(int invoiceID, string status);
        public event EventHandler ItemClick;
        public class InvoiceDetailViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public FullObservableCollection<DetailInvoice> detailInvoices { get; set; }
            IDao _dao;
            public Invoice _invoice { get; set; }
            public Visibility ButtonVisibility => _invoice?.Status == "Wait" ? Visibility.Visible : Visibility.Collapsed;

            public InvoiceDetailViewModel()
            {
                _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
                detailInvoices = new FullObservableCollection<DetailInvoice>();
            }
            public void SetDetailInvoices(Invoice invoice)
            {
                _invoice = invoice;
                detailInvoices = new FullObservableCollection<DetailInvoice>(_dao.GetDetailInvoicesOfId(invoice.InvoiceID));
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
        InvoiceDetailViewModel ViewModel { get; set; }
        public InvoiceDetailUserControl()
        {
            this.InitializeComponent();
            ViewModel = new InvoiceDetailViewModel();
        }

        public void SetInvoiceDetails(Invoice invoice)
        {
          //  InvoiceIDTextBlock.Text = $"Invoice ID: {invoice.InvoiceID}";
        //    CustomerNameTextBlock.Text = $"Customer: {invoice.CustomerName}";
        //    TotalAmountTextBlock.Text = $"Total Amount: {invoice.TotalAmount}";
            ViewModel.SetDetailInvoices(invoice);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.setStatus("Cancel");
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.setStatus("Paid");
        }
    }
}
