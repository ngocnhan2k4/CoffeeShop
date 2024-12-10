using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.ViewModels.InvoicePage;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.UserControls.InvoicePage
{
    public sealed partial class InvoiceListUserControl : UserControl
    {
        public delegate void EventHandler(Invoice invoice);
        public event EventHandler ItemClick;

  
        InvoiceListViewModel ViewModel { get; set; }
        public InvoiceListUserControl()
        {
            this.InitializeComponent();
            ViewModel = new InvoiceListViewModel();

            // Select the first item by default if there are items
            if (ViewModel.invoices != null && ViewModel.invoices.Count > 0)
            {
                InvoiceListView.SelectedIndex = 0;
            }
        }
        private void InvoiceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InvoiceListView.SelectedItem is Invoice selectedInvoice)
            {
                ItemClick?.Invoke(selectedInvoice);
            }
        }

        public void SetInvoice(int invoiceID, string status)
        {
            var invoice = ViewModel.invoices.FirstOrDefault(i => i.InvoiceID == invoiceID);
            if (invoice != null)
            {
                invoice.Status = status;
            }
        }
    }
}
