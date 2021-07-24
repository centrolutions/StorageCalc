using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using StorageCalc.Calculators;
using StorageCalc.ViewModels;
using Xunit;

namespace StorageCalc.Tests.ViewModels
{
    public class ViewModelLocatorTests
    {
        ViewModelLocator _Sut;

        public ViewModelLocatorTests()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IMessageBoxHelper>(new MessageBoxHelper())
                    .AddSingleton<MainWindowViewModel>()
                    .AddSingleton<IRaidCalculatorFactory, RaidCalculatorFactory>()
                    .BuildServiceProvider()
                );

            _Sut = new ViewModelLocator();
        }

        [Fact]
        public void MainWindowViewModel_IsNotNull_WhenIocContainsInstance()
        {
            var result = _Sut.MainWindowViewModel;

            result.Should().NotBeNull();
        }
    }
}
