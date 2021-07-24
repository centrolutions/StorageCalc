using FluentAssertions;
using StorageCalc.Calculators;
using StorageCalc.Resources;
using Xunit;

namespace StorageCalc.Tests.Calculators
{
    public class Raid1CalculatorTests
    {
        Raid1Calculator _Sut = new Raid1Calculator();

        [Theory]
        [InlineData(2, 1, 0.91, "de-CH", "1 Platte")]
        [InlineData(2, 1, 0.91, "en-US", "1 Disk")]
        public void Calculate_ReturnsExpectedValues_WhenCalled(int diskCount, double diskSpace, double expectedTotalSpace, string cultureCode, string expectedFaultTolerance)
        {
            LocalizedStrings.Instance.SetCulture(cultureCode);

            var result = _Sut.Calculate(diskCount, diskSpace);

            result.Should().NotBeNull();
            result.ErrorMessage.Should().BeNullOrWhiteSpace();
            result.UseableDiskSpace.Should().BeApproximately(expectedTotalSpace, 0.01);
            result.FaultTolerance.Should().Be(expectedFaultTolerance);
        }

        [Fact]
        public void Calculate_ReturnsErrorResult_WhenDiskCountIsNot2()
        {
            var result = _Sut.Calculate(1, 1);

            result.Should().NotBeNull();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void RaidNumber_IsSetToOne()
        {
            _Sut.RaidNumber.Should().Be(1);
        }
    }
}
