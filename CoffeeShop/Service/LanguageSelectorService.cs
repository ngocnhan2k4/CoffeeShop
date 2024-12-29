using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Data;

namespace CoffeeShop.Service
{
    public interface ILanguageSelectorService
    {
        void SetLanguage(string languageCode);
        string GetCurrentLanguage();
        event EventHandler LanguageChanged;
    }

    public class LanguageSelectorService : ILanguageSelectorService
    {
        private readonly Dictionary<string, string> _languageMapping = new()
        {
            { "English", "en-US" },
            { "Vietnamese", "vi-VN" }
        };

        public event EventHandler LanguageChanged;

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
            
            LanguageChanged?.Invoke(this, EventArgs.Empty);

            // Force UI update
            var app = Application.Current as App;
            if (app?.MainWindow?.Content is Frame rootFrame)
            {
                var currentPage = rootFrame.Content;
                if (currentPage != null)
                {
                    var pageType = currentPage.GetType();
                    
                    // Store current state if needed
                    var currentState = new Dictionary<string, object>();
                    if (currentPage is FrameworkElement element)
                    {
                        foreach (var property in element.GetType().GetProperties())
                        {
                            if (property.CanRead && property.CanWrite)
                            {
                                currentState[property.Name] = property.GetValue(element);
                            }
                        }
                    }

                    // Navigate to force refresh
                    rootFrame.Navigate(pageType);

                    // Restore state if needed
                    if (rootFrame.Content is FrameworkElement newElement)
                    {
                        foreach (var pair in currentState)
                        {
                            var property = newElement.GetType().GetProperty(pair.Key);
                            if (property?.CanWrite == true)
                            {
                                property.SetValue(newElement, pair.Value);
                            }
                        }
                        newElement.UpdateLayout();
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
