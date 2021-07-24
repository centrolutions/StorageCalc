using FluentAssertions;
using StorageCalc.Calculators;
using StorageCalc.Resources;
using Xunit;

namespace StorageCalc.Tests.Calculators
{
    public class Raid10CalculatorTests
    {
        Raid10Calculator _Sut = new Raid10Calculator();

        [Theory]
        [InlineData(4, 1, 1.82, "de-CH", "Min. 1 Platte")]
        [InlineData(4, 1, 1.82, "en-US", "Min. 1 Disk")]
        public void Calculate_ReturnsExpectedValues_WhenCalled(int diskCount, double diskSpace, double expectedTotalSpace, string cultureCode, string expectedFaultTolerance)
        {
            LocalizedStrings.Instance.SetCulture(cultureCode);
            var result = _Sut.Calculate(diskCount, diskSpace);

            result.Should().NotBeNull();
            result.ErrorMessage.Should().BeNullOrWhiteSpace();
            result.UseableDiskSpace.Should().BeApproximately(expectedTotalSpace, 0.01);
            result.FaultTolerance.Should().Be(expectedFaultTolerance);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(9)]
        public void Calculate_ReturnsErrorResult_WhenDiskCountNotAMultipleOfTwo(int diskCount)
        {
            var result = _Sut.Calculate(diskCount, 1);

            result.Should().NotBeNull();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(2)]
        [InlineData(1)]
        public void Calculate_ReturnsErrorResult_WhenDiskCountIsLessThan4(int diskCount)
        {
            var result = _Sut.Calculate(diskCount, 1);

            result.Should().NotBeNull();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void RaidNumber_IsSetToTen()
        {
            _Sut.RaidNumber.Should().Be(10);
        }
    }
}
