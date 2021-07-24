using FluentAssertions;
using Moq;
using StorageCalc.Calculators;
using StorageCalc.ViewModels;
using System.Collections.Generic;
using Xunit;

namespace StorageCalc.Tests.ViewModels
{
    public class MainWindowViewModelTests
    {
        Mock<IMessageBoxHelper> _MessageBoxMock;
        IMessageBoxHelper _MessageBox;
        Mock<IRaidCalculatorFactory> _RaidCalculatorFactoryMock;
        IRaidCalculatorFactory _RaidCalculatorFactory;
        Mock<IRaidCalculator> _RaidCalculatorMock;
        IRaidCalculator _RaidCalculator;
        RaidCalculatorResult _RaidCalculatorResult = new RaidCalculatorResult() { FaultTolerance = "none", UseableDiskSpace = 1.23 };

        MainWindowViewModel _Sut;


        public MainWindowViewModelTests()
        {

            _MessageBoxMock = new Mock<IMessageBoxHelper>();
            _MessageBox = _MessageBoxMock.Object;

            _RaidCalculatorMock = new Mock<IRaidCalculator>();
            _RaidCalculatorMock.Setup(x => x.Calculate(It.IsAny<int>(), It.IsAny<double>())).Returns(_RaidCalculatorResult);
            _RaidCalculator = _RaidCalculatorMock.Object;

            _RaidCalculatorFactoryMock = new Mock<IRaidCalculatorFactory>();
            _RaidCalculatorFactoryMock.Setup(x => x.GetAll()).Returns(new List<IRaidCalculator>() { _RaidCalculator });
            _RaidCalculatorFactory = _RaidCalculatorFactoryMock.Object;

            _Sut = new MainWindowViewModel(_MessageBox, _RaidCalculatorFactory);
            _Sut.SelectedCalculator = _RaidCalculator;
        }

        [Theory]
        [InlineData(0, 1, true)]
        [InlineData(1, 0, true)]
        [InlineData(2, 2, false)]
        public void CalculateCommand_CannotExecute_WhenParameterIsInvalid(int diskCount, double diskSpace, bool hasSelectedRaid)
        {
            _Sut.DiskCount = diskCount;
            _Sut.DiskSpace = diskSpace;
            if (!hasSelectedRaid)
                _Sut.SelectedCalculator = null;

            var result = _Sut.CalculateCommand.CanExecute(null);

            result.Should().BeFalse();
        }

        [Fact]
        public void Calculators_IsNotEmpty_WhenClassIsInitialized()
        {
            Assert.NotEmpty(_Sut.Calculators);
        }

        [Fact]
        public void CalculateCommand_UsesResultsFromSelectedCalculator_WhenSelectedCalculatorIsNotNull()
        {
            _Sut.DiskCount = 2;
            _Sut.DiskSpace = 2;

            _Sut.CalculateCommand.Execute(null);

            _Sut.FaultToleranceText.Should().Be(_RaidCalculatorResult.FaultTolerance);
            _Sut.TotalSpaceText.Should().Be($"{_RaidCalculatorResult.UseableDiskSpace} TB");
        }

        [Fact]
        public void CalculateCommand_ShowsErrorMessage_WhenErrorIsReturnedFromCalculator()
        {
            _RaidCalculatorMock.Setup(x => x.Calculate(It.IsAny<int>(), It.IsAny<double>())).Returns(new RaidCalculatorResult() { ErrorMessage = "This is an error" });
            _Sut.DiskCount = 2;
            _Sut.DiskSpace = 2;

            _Sut.CalculateCommand.Execute(null);

            _Sut.FaultToleranceText.Should().BeNullOrWhiteSpace();
            _Sut.TotalSpaceText.Should().BeNullOrWhiteSpace();
            _MessageBoxMock.Verify(x => x.Show("This is an error"));
        }
    }
}
