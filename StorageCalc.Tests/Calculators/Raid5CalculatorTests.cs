using FluentAssertions;
using StorageCalc.Calculators;
using StorageCalc.Resources;
using Xunit;

namespace StorageCalc.Tests.Calculators
{
    public class Raid5CalculatorTests
    {
        Raid5Calculator _Sut = new Raid5Calculator();

        [Theory]
        [InlineData(3, 2, 3.64, "de-CH", "1 Platte")]
        [InlineData(3, 2, 3.64, "en-US", "1 Disk")]
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
        [InlineData(2)]
        [InlineData(1)]
        public void Calculate_ReturnsErrorResult_WhenDiskCountIsLessThan3(int diskCount)
        {
            var result = _Sut.Calculate(diskCount, 1);

            result.Should().NotBeNull();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void RaidNumber_IsSetToFive()
        {
            _Sut.RaidNumber.Should().Be(5);
        }
    }
}
