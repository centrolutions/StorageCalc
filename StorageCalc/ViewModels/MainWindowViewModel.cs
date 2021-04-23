using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using StorageCalc.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCalc.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        IMessageBoxHelper messageBox;

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

        private bool _Raid0;
        public bool Raid0
        {
            get => _Raid0;
            set => SetProperty(ref _Raid0, value);
        }

        private bool _Raid1;
        public bool Raid1
        {
            get => _Raid1;
            set => SetProperty(ref _Raid1, value);
        }

        private bool _Raid5;
        public bool Raid5
        {
            get => _Raid5;
            set => SetProperty(ref _Raid5, value);
        }

        private bool _Raid6;
        public bool Raid6
        {
            get => _Raid6;
            set => SetProperty(ref _Raid6, value);
        }

        private bool _Raid10;
        public bool Raid10
        {
            get => _Raid10;
            set => SetProperty(ref _Raid10, value);
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

        public RelayCommand CalculateCommand { get; init; }

        public MainWindowViewModel(IMessageBoxHelper messageBox)
        {
            this.messageBox = messageBox;
            CalculateCommand = new RelayCommand(Calculate, CanCalculate);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CalculateCommand.NotifyCanExecuteChanged();

        }

        private bool CanCalculate()
        {
            return DiskCount > 0 && DiskSpace > 0
                && (Raid0 || Raid1 || Raid5 || Raid6 || Raid10);
        }

        private void Calculate()
        {
            var diskspaceInBytes = DiskSpace * 1000000000000L;
            var divisor = 1024L * 1024L * 1024L * 1024L;
            var realDiskSpace = (double)diskspaceInBytes / (double)divisor;

            var usableSpace = 0.0d;
            var faultTolerance = string.Empty;

            if (Raid0)
            {
                usableSpace = DiskCount * realDiskSpace;
                faultTolerance = LocalizedStrings.Instance["None"];
            }

            if (Raid1)
            {
                if (DiskCount == 2)
                {
                    usableSpace = realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["OneDisk"];
                }
                else
                {
                    messageBox.Show(LocalizedStrings.Instance["ExactlyTwoPlatesAreRequired"]);
                    return;
                }
            }

            if (Raid5)
            {
                if (DiskCount >= 3)
                {
                    usableSpace = (DiskCount - 1) * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["OneDisk"];
                }
                else
                {
                    messageBox.Show(LocalizedStrings.Instance["AtLeastThreePlatesRequired"]);
                    return;
                }
            }

            if (Raid6)
            {
                if (DiskCount >= 4)
                {
                    usableSpace = (DiskCount - 2) * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["TwoDisks"];
                }
                else
                {
                    messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesRequired"]);
                    return;
                }
            }

            if (Raid10)
            {
                if (DiskCount % 2 == 0 && DiskCount >= 4)
                {
                    usableSpace = (DiskCount - 2) * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["MinOneDisk"];
                }
                else
                {
                    messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesAndEvenNumber"]);
                    return;
                }
            }

            TotalSpaceText = Math.Round(usableSpace, 2) + " TB";
            FaultToleranceText = faultTolerance;
        }
    }
}
