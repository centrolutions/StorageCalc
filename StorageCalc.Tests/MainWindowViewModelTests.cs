using Moq;
using StorageCalc.Resources;
using StorageCalc.ViewModels;
using System;
using Xunit;

namespace StorageCalc.Tests
{
    public class MainWindowViewModelTests
    {
        Mock<IMessageBoxHelper> messageBoxMock;
        IMessageBoxHelper messageBox;
        MainWindowViewModel _sut;

        public MainWindowViewModelTests()
        {
            
            messageBoxMock = new Mock<IMessageBoxHelper>();
            messageBox = messageBoxMock.Object;

            _sut = new MainWindowViewModel(messageBox);
        }

        [Theory]
        [InlineData("2", "4", true, null, null, null, null, "7.28 TB", "keine", "de-CH")]
        [InlineData("2", "4", true, null, null, null, null, "7.28 TB", "none", "en-US")]
        [InlineData("2", "1", null, true, null, null, null, "0.91 TB", "1 Platte", "de-CH")]
        [InlineData("3", "2", null, null, true, null, null, "3.64 TB", "1 Platte", "de-CH")]
        [InlineData("4", "1", null, null, null, true, null, "1.82 TB","2 Platten", "de-CH")]
        [InlineData("4", "1", null, null, null, null, true, "1.82 TB", "Min. 1 Platte", "de-CH")]
        public void Calculate_ReturnsExpectedStrings_WhenGoodDataIsPassed(string txtDiskCount, string txtDiskSpace, bool? raid0, bool? raid1, bool? raid5, bool? raid6, bool? raid10, string expectedTotalSpace, string expectedFaultTolerance, string cultureCode)
        {
            LocalizedStrings.Instance.SetCulture(cultureCode);
            (string totalSpace, string faultTolerance) = _sut.Calculate(txtDiskCount, txtDiskSpace, raid0, raid1, raid5, raid6, raid10);

            Assert.Equal(expectedTotalSpace, totalSpace);
            Assert.Equal(expectedFaultTolerance, faultTolerance);
        }

        [Theory]
        [InlineData("1", "1", null, true, null, null, null)]
        [InlineData("1", "1", null, null, true, null, null)]
        [InlineData("1", "1", null, null, null, true, null)]
        [InlineData("1", "1", null, null, null, null, true)]
        [InlineData("abc", "1", true, null, null, null, null)]
        [InlineData("999999999999", "1", true, null, null, null, null)]
        public void Calculate_ShowsErrorMessageBox_WhenBadDataIsPassed(string txtDiskCount, string txtDiskSpace, bool? raid0, bool? raid1, bool? raid5, bool? raid6, bool? raid10)
        {
            (string totalSpace, string faultTolerance) = _sut.Calculate(txtDiskCount, txtDiskSpace, raid0, raid1, raid5, raid6, raid10);

            Assert.Equal(default(string), totalSpace);
            Assert.Equal(default(string), faultTolerance);
            messageBoxMock.Verify(m => m.Show(It.IsAny<string>()));
        }
    }
}
