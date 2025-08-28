using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SkolaProgramiranja.Models
{
    class LanguageManager
    {
        public static string CurrentLanguage { get; private set; } = "sr";
        public static void ChangeLanguage(string langCode)
        {
            var dict = new ResourceDictionary();
            switch (langCode)
            {
                case "en":
                    dict.Source = new System.Uri("Languages/Strings.en.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new System.Uri("Languages/Strings.sr.xaml", UriKind.Relative);
                    break;
            }

            // Pronađi i zamijeni postojeći jezički rječnik bez brisanja ostalih
            var existingLangDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.StartsWith("Languages/Strings"));

            if (existingLangDict != null)
            {
                int index = Application.Current.Resources.MergedDictionaries.IndexOf(existingLangDict);
                Application.Current.Resources.MergedDictionaries[index] = dict;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }

            CultureInfo culture = new CultureInfo(langCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            CurrentLanguage = langCode;
        }


        public static void ApplyCurrentLanguage()
        {
            ChangeLanguage(CurrentLanguage);
        }
    }
}   
