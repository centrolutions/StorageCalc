using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using StorageCalc.Calculators;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StorageCalc.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IMessageBoxHelper _MessageBox;
        private readonly IRaidCalculatorFactory _CalculatorFactory;

        private int _DiskCount;
        public int DiskCount
        {
            get => _DiskCount;
            set => SetProperty(ref _DiskCount, value);
        }

        private double _DiskSpace;
        public double DiskSpace
        {
            get => _DiskSpace;
            set => SetProperty(ref _DiskSpace, value);
        }

        private string _TotalSpaceText;
        public string TotalSpaceText
        {
            get => _TotalSpaceText;
            set => SetProperty(ref _TotalSpaceText, value);
        }

        private string _FaultToleranceText;
        public string FaultToleranceText
        {
            get => _FaultToleranceText;
            set => SetProperty(ref _FaultToleranceText, value);
        }

        public ObservableCollection<IRaidCalculator> Calculators { get; init; }

        private IRaidCalculator _SelectedCalculator;
        public IRaidCalculator SelectedCalculator
        {
            get => _SelectedCalculator;
            set => SetProperty(ref _SelectedCalculator, value);
        }

        public RelayCommand CalculateCommand { get; init; }

        public MainWindowViewModel(IMessageBoxHelper messageBox, IRaidCalculatorFactory calculatorFactory)
        {
            this._MessageBox = messageBox;
            _CalculatorFactory = calculatorFactory;
            CalculateCommand = new RelayCommand(Calculate, CanCalculate);
            Calculators = new ObservableCollection<IRaidCalculator>(_CalculatorFactory.GetAll());
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CalculateCommand.NotifyCanExecuteChanged();

        }

        private bool CanCalculate()
        {
            return DiskCount > 0 && DiskSpace > 0
                && SelectedCalculator != null;
        }

        private void Calculate()
        {
            var result = SelectedCalculator.Calculate(DiskCount, DiskSpace);

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                _MessageBox.Show(result.ErrorMessage);
                return;
            }
            TotalSpaceText = $"{Math.Round(result.UseableDiskSpace, 2)} TB";
            FaultToleranceText = result.FaultTolerance;
        }
    }
}
