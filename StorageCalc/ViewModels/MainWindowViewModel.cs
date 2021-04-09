using StorageCalc.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCalc.ViewModels
{
    public class MainWindowViewModel
    {
        IMessageBoxHelper messageBox;

        public MainWindowViewModel(IMessageBoxHelper messageBox)
        {
            this.messageBox = messageBox;
        }

        public (string TotalSize, string FaulTolerance) Calculate(string txtDiskCount, string txtDiskSpace, bool? raid0, bool? raid1, bool? raid5, bool? raid6, bool? raid10)
        {
            try
            {
                var diskCount = Convert.ToInt32(txtDiskCount);
                var diskSpace = Convert.ToDouble(txtDiskSpace);

                var diskspaceInBytes = diskSpace * 1000000000000L;
                var divisor = 1024L * 1024L * 1024L * 1024L;
                var realDiskSpace = (double)diskspaceInBytes / (double)divisor;

                var usableSpace = 0.0d;
                var faultTolerance = string.Empty;

                if (raid0 == true)
                {
                    usableSpace = diskCount * realDiskSpace;
                    faultTolerance = LocalizedStrings.Instance["None"];
                }

                if (raid1 == true)
                {
                    if (diskCount == 2)
                    {
                        usableSpace = realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["OneDisk"];
                    }
                    else
                    {
                        messageBox.Show(LocalizedStrings.Instance["ExactlyTwoPlatesAreRequired"]);
                        return default;
                    }
                }

                if (raid5 == true)
                {
                    if (diskCount >= 3)
                    {
                        usableSpace = (diskCount - 1) * realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["OneDisk"];
                    }
                    else
                    {
                        messageBox.Show(LocalizedStrings.Instance["AtLeastThreePlatesRequired"]);
                        return default;
                    }
                }

                if (raid6 == true)
                {
                    if (diskCount >= 4)
                    {
                        usableSpace = (diskCount - 2) * realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["TwoDisks"];
                    }
                    else
                    {
                        messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesRequired"]);
                        return default;
                    }
                }

                if (raid10 == true)
                {
                    if (diskCount % 2 == 0 && diskCount >= 4)
                    {
                        usableSpace = (diskCount - 2) * realDiskSpace;
                        faultTolerance = LocalizedStrings.Instance["MinOneDisk"];
                    }
                    else
                    {
                        messageBox.Show(LocalizedStrings.Instance["AtLeastFourPlatesAndEvenNumber"]);
                        return default;
                    }
                }

                return(Math.Round(usableSpace, 2) + " TB", faultTolerance);
            }
            catch (FormatException)
            {
                messageBox.Show(LocalizedStrings.Instance["PleaseOnlyEnterNumber"]);
                return default;
            }
            catch (OverflowException)
            {
                messageBox.Show(LocalizedStrings.Instance["PleaseEnterSmallerNumbers"]);
                return default;
            }
        }
    }
}
