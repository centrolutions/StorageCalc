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
        [InlineData(2, 4, true, false, false, false, false, "7.28 TB", "keine", "de-CH")]
        [InlineData(2, 4, true, false, false, false, false, "7.28 TB", "none", "en-US")]
        [InlineData(2, 1, false, true, false, false, false, "0.91 TB", "1 Platte", "de-CH")]
        [InlineData(3, 2, false, false, true, false, false, "3.64 TB", "1 Platte", "de-CH")]
        [InlineData(4, 1, false, false, false, true, false, "1.82 TB","2 Platten", "de-CH")]
        [InlineData(4, 1, false, false, false, false, true, "1.82 TB", "Min. 1 Platte", "de-CH")]
        public void Calculate_ReturnsExpectedStrings_WhenGoodDataIsPassed(int diskCount, double diskSpace, bool raid0, bool raid1, bool raid5, bool raid6, bool raid10, string expectedTotalSpace, string expectedFaultTolerance, string cultureCode)
        {
            LocalizedStrings.Instance.SetCulture(cultureCode);
            _sut.DiskCount = diskCount;
            _sut.DiskSpace = diskSpace;
            _sut.Raid0 = raid0;
            _sut.Raid1 = raid1;
            _sut.Raid5 = raid5;
            _sut.Raid6 = raid6;
            _sut.Raid10 = raid10;

            if (_sut.CalculateCommand.CanExecute(null))
                _sut.CalculateCommand.Execute(null);

            Assert.Equal(expectedTotalSpace, _sut.TotalSpaceText);
            Assert.Equal(expectedFaultTolerance, _sut.FaultToleranceText);
        }

        [Theory]
        [InlineData(1, 1, false, true, false, false, false)]
        [InlineData(1, 1, false, false, true, false, false)]
        [InlineData(1, 1, false, false, false, true, false)]
        [InlineData(1, 1, false, false, false, false, true)]
        public void Calculate_ShowsErrorMessageBox_WhenBadDataIsPassed(int diskCount, double diskSpace, bool raid0, bool raid1, bool raid5, bool raid6, bool raid10)
        {
            _sut.DiskCount = diskCount;
            _sut.DiskSpace = diskSpace;
            _sut.Raid0 = raid0;
            _sut.Raid1 = raid1;
            _sut.Raid5 = raid5;
            _sut.Raid6 = raid6;
            _sut.Raid10 = raid10;

            if (_sut.CalculateCommand.CanExecute(null))
                _sut.CalculateCommand.Execute(null);

            Assert.Equal(default(string), _sut.TotalSpaceText);
            Assert.Equal(default(string), _sut.FaultToleranceText);
            messageBoxMock.Verify(m => m.Show(It.IsAny<string>()));
        }
    }
}
