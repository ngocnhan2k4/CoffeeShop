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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoffeeShop.Views.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangeLanguagePage : Page
    {
        private Dictionary<TextBlock, string> resourceMapping;
        private void InitializeResourceMapping()
        {
            // Khởi tạo dictionary ánh xạ TextBlock -> tên tài nguyên
            resourceMapping = new Dictionary<TextBlock, string>
            {
                { myGreeting, "TongTienNhapKho" },
            };
        }
        public ChangeLanguagePage()
        {
            this.InitializeComponent();
            InitializeResourceLoader("vi-VN");
        }
        private void InitializeResourceLoader(string language)
        {
            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext();
            resourceContext.QualifierValues["Language"] = language;
            var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            var resourceCandidate = resourceMap.GetValue("TongTienNhapKho", resourceContext);
            if (resourceCandidate != null)
            {
                this.myGreeting.Text = resourceCandidate.ValueAsString;
            }
            else
            {
                this.myGreeting.Text = "Resource not found!";
            }
        }
        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLanguage = (cbLanguages.SelectedItem as ComboBoxItem)?.Tag.ToString();
            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                InitializeResourceLoader(selectedLanguage);
            }
        }

    }
   

}
