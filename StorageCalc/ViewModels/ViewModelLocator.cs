using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCalc.ViewModels
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return Ioc.Default.GetService<MainWindowViewModel>();
            }
        }
    }
}
