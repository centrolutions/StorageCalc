using System.Collections.Generic;

namespace StorageCalc.Calculators
{
    public interface IRaidCalculatorFactory
    {
        IReadOnlyList<IRaidCalculator> GetAll();
    }
}
