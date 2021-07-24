using Microsoft.Toolkit.Mvvm.DependencyInjection;

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
