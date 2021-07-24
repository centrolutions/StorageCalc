using FluentAssertions;
using StorageCalc.Calculators;
using StorageCalc.Resources;
using Xunit;

namespace StorageCalc.Tests.Calculators
{
    public class Raid6CalculatorTests
    {
        Raid6Calculator _Sut = new Raid6Calculator();

        [Theory]
        [InlineData(4, 1, 1.82, "de-CH", "2 Platten")]
        [InlineData(4, 1, 1.82, "en-US", "2 Disks")]
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
        public void RaidNumber_IsSetToSix()
        {
            _Sut.RaidNumber.Should().Be(6);
        }
    }
}
