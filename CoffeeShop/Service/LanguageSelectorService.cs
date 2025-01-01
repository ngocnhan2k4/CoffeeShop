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

            // Force UI update for the entire app
            var app = Application.Current as App;
            if (app?.MainWindow?.Content is NavigationView mainNav)
            {
                // Update NavigationView items
                foreach (var item in mainNav.MenuItems)
                {
                    if (item is NavigationViewItem navItem)
                    {
                        navItem.UpdateLayout();
                    }
                }

                // Update current page content
                if (mainNav.Content is Frame mainFrame)
                {
                    var currentPage = mainFrame.Content;
                    if (currentPage != null)
                    {
                        var pageType = currentPage.GetType();

                        // Store current state
                        var currentState = new Dictionary<string, object>();
                        if (currentPage is FrameworkElement element)
                        {
                            foreach (var property in element.GetType().GetProperties())
                            {
                                if (property.CanRead && property.CanWrite)
                                {
                                    try
                                    {
                                        currentState[property.Name] = property.GetValue(element);
                                    }
                                    catch { }
                                }
                            }
                        }

                        // Navigate to force refresh
                        mainFrame.Navigate(pageType);

                        // Restore state
                        if (mainFrame.Content is FrameworkElement newElement)
                        {
                            foreach (var pair in currentState)
                            {
                                var property = newElement.GetType().GetProperty(pair.Key);
                                if (property?.CanWrite == true)
                                {
                                    try
                                    {
                                        property.SetValue(newElement, pair.Value);
                                    }
                                    catch { }
                                }
                            }

                            // If this is SettingsPage, update its NavigationView
                            if (newElement is Page settingsPage)
                            {
                                var settingsNav = settingsPage.FindName("NavView") as NavigationView;
                                if (settingsNav != null)
                                {
                                    // Update NavigationView items
                                    foreach (var item in settingsNav.MenuItems)
                                    {
                                        if (item is NavigationViewItem navItem)
                                        {
                                            // Force update bindings
                                            var content = navItem.Content;
                                            var tag = navItem.Tag;
                                            navItem.Content = null;
                                            navItem.Tag = null;
                                            navItem.Content = content;
                                            navItem.Tag = tag;
                                            navItem.UpdateLayout();
                                        }
                                    }

                                    // Update current settings page
                                    if (settingsNav.Content is Frame settingsFrame && 
                                        settingsFrame.Content != null)
                                    {
                                        var currentSettingsPage = settingsFrame.Content;
                                        var settingsPageType = currentSettingsPage.GetType();
                                        settingsFrame.Navigate(settingsPageType);
                                    }
                                }
                            }
                            newElement.UpdateLayout();
                        }
                    }
                }
            }
        }

        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is T found)
                {
                    return found;
                }
                
                var result = FindChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public string GetCurrentLanguage()
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            return _languageMapping.FirstOrDefault(x => x.Value == currentCulture).Key ?? "English";
        }
    }
}
