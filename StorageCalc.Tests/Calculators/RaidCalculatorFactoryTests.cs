using FluentAssertions;
using StorageCalc.Calculators;
using System.Linq;
using Xunit;

namespace StorageCalc.Tests.Calculators
{
    public class RaidCalculatorFactoryTests
    {
        RaidCalculatorFactory _Sut = new RaidCalculatorFactory();

        [Fact]
        public void GetAll_ReturnsFiveInstances_AfterInitialized()
        {
            var result = _Sut.GetAll().Count;

            result.Should().Be(5);
        }

        [Fact]
        public void GetAll_FirstInstanceIsRaid0_WhenCalled()
        {
            var first = _Sut.GetAll().First();

            first.RaidNumber.Should().Be(0);
            first.Name.Should().Be("RAID 0");
        }

        [Fact]
        public void GetAll_LastInstanceIsRaid10_WhenCalled()
        {
            var last = _Sut.GetAll().Last();

            last.RaidNumber.Should().Be(10);
            last.Name.Should().Be("RAID 10");
        }
    }
}
