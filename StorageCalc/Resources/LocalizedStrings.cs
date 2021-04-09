using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace StorageCalc.Resources
{
    public class LocalizedStrings
    {
        private LocalizedStrings()
        {

        }

        public static LocalizedStrings Instance { get; } = new LocalizedStrings();

        public void SetCulture(string cultureCode)
        {
            var newCulture = new CultureInfo(cultureCode);
            LocalizeDictionary.Instance.Culture = newCulture;
        }

        public string this[string key]
        {
            get
            {
                var result = LocalizeDictionary.Instance.GetLocalizedObject("StorageCalc", "Strings", key, LocalizeDictionary.Instance.Culture);
                return result as string;
            }
        }
    }
}
