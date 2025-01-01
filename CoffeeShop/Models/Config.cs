using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoffeeShop.Models
{
    /// <summary>
    /// This class represents an invoice
    /// </summary>
    public class Config : INotifyPropertyChanged
    {
        public string Theme { get; set; }
        public string Language { get; set; }

        public Config()
        {
            //Theme = "Light";
            //Language = "English";
            //FontSize = "16";
        }

        public Config(Config other)
        {
            this.Theme = other.Theme;
            this.Language = other.Language;
        }

        public Config CopyFrom(Config source)
        {
            if (source != null)
            {
                this.Theme = source.Theme;
                this.Language = source.Language;
            }
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}