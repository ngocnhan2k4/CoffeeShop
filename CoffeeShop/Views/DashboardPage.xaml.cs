using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Charts;
using System.Collections.ObjectModel;
using CoffeeShop.ViewModels;
using System.Threading.Tasks;
using System.Diagnostics;
using CoffeeShop.Service.BusinessLogic;
using System.ComponentModel;
using Microsoft.UI;
using CoffeeShop.Service.DataAccess;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public DashboardViewModel SalesDashboard { get; set; }
    
        public DashboardPage()
        {
            this.InitializeComponent();
        
            SalesDashboard = new DashboardViewModel(DateTime.Now.Year);    
            yearDatePicker.MinYear = new DateTimeOffset(new DateTime(SalesDashboard.SaleService.Years[1], 1, 1));
            yearDatePicker.MaxYear = new DateTimeOffset(new DateTime(SalesDashboard.SaleService.Years[0], 1, 1));
            yearDatePicker.Date = new DateTimeOffset(new DateTime(DateTime.Now.Year, yearDatePicker.Date.Month, yearDatePicker.Date.Day));
           
            RefreshCharts();
           
        }

        private  void YearDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            int selectedYear = e.NewDate.Year;

            // Update the SalesService with the new year
            SalesDashboard.SaleService = new SalesService(SalesDashboard.SaleService.dao, selectedYear);

            // Notify the UI that the properties have changed
            OnPropertyChanged(nameof(SalesDashboard.SaleService.TotalCost));
            OnPropertyChanged(nameof(SalesDashboard.SaleService.Revenue));
            OnPropertyChanged(nameof(SalesDashboard.SaleService.Profit));
            OnPropertyChanged(nameof(SalesDashboard.SaleService.Years));
            OnPropertyChanged(nameof(SalesDashboard.SaleService.NumberOrders));
            
            RefreshCharts();
            SalesDashboard.TopDrink.Clear();
        /*    foreach (var drink in SalesDashboard.SaleService.CalculateTopDrinks(SalesDashboard.SaleService.dao, selectedYear))
            {
                SalesDashboard.TopDrink.Add(drink); 
            }*/
            foreach (var drink in SalesDashboard.SaleService.dao.CalculateTopDrinks( selectedYear))
            {
                SalesDashboard.TopDrink.Add(drink);
            }

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void RefreshCharts()
        {
            // Clear existing series
            RevenueChart.Series.Clear();
            CategoryRevenueChart.Series.Clear();

            // Create new series and set the ItemsSource
            var revenueSeries = new ColumnSeries
            {
                ItemsSource = SalesDashboard.SaleService.MonthlyRevenue.Select((revenue, index) => new { Month = index + 1, Revenue = revenue }).ToList(),
                XBindingPath = "Month",
                YBindingPath = "Revenue",
                EnableTooltip = true,
               
            };
            var categoryRevenueSeries = new PieSeries
            {
                ItemsSource = SalesDashboard.SaleService.RevenueByCategory.Select(kv => new { Category = kv.Key, SalesAmount = kv.Value }).ToList(),
                XBindingPath = "Category",
                YBindingPath = "SalesAmount",
                EnableTooltip = true,
                ShowDataLabels = true
            };

            // Add the series to the charts
            RevenueChart.Series.Add(revenueSeries);
            CategoryRevenueChart.Series.Add(categoryRevenueSeries);
        }
    }
}