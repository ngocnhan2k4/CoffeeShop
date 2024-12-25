using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace CoffeeShop.Service
{
    public interface ILanguageSelectorService
    {
        void SetLanguage(string languageCode);
        string GetCurrentLanguage();
    }

    public class LanguageSelectorService : ILanguageSelectorService
    {
        private readonly Dictionary<string, string> _languageMapping = new()
        {
            { "English", "en-US" },
            { "Vietnamese", "vi-VN" }
        };

        public void SetLanguage(string language)
        {
            if (!_languageMapping.ContainsKey(language))
                return;

            var cultureName = _languageMapping[language];
            var culture = new CultureInfo(cultureName);
            
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri($"ms-appx:///Resources/Strings.{cultureName}.xaml")
            };

            var resources = Application.Current.Resources.MergedDictionaries;
            for (int i = resources.Count - 1; i >= 0; i--)
            {
                var resource = resources[i];
                if (resource.Source?.OriginalString.Contains("/Resources/Strings.") == true)
                {
                    resources.RemoveAt(i);
                }
            }
            
            resources.Add(resourceDictionary);

            // Force UI update
            var app = Application.Current as App;
            if (app?.MainWindow?.Content is NavigationView navView)
            {
                var frame = navView.Content as Frame;
                if (frame != null)
                {
                    var currentPage = frame.Content;
                    var pageType = currentPage.GetType();
                    frame.Navigate(pageType); // Navigate to the same page type to force refresh
                    frame.NavigationFailed += (s, e) =>
                    {
                        // If navigation fails, try to recreate the page
                        var newPage = Activator.CreateInstance(pageType);
                        frame.Content = newPage;
                    };
                }

                // Update NavigationView items
                foreach (var item in navView.MenuItems)
                {
                    if (item is NavigationViewItem navItem)
                    {
                        navItem.UpdateLayout();
                    }
                }
            }
        }

        public string GetCurrentLanguage()
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            return _languageMapping.FirstOrDefault(x => x.Value == currentCulture).Key ?? "English";
        }
    }
}
