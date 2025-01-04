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
using CoffeeShop.ViewModels.InvoicePage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.InvoicePage
{
    public sealed partial class InvoiceDetailUserControl : UserControl
    {
        public delegate void EventHandler(int invoiceID, string status);
        public event EventHandler ItemClick;
     
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
