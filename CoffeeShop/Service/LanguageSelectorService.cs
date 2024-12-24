using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.UI.Xaml;

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
        }

        public string GetCurrentLanguage()
        {
            return CultureInfo.CurrentUICulture.Name;
        }
    }
}
