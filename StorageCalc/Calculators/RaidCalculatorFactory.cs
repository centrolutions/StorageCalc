using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageCalc.Calculators
{
    public class RaidCalculatorFactory : IRaidCalculatorFactory
    {
        private readonly IReadOnlyList<IRaidCalculator> _RaidCalculators;
        public RaidCalculatorFactory()
        {
            var calcType = typeof(IRaidCalculator);
            _RaidCalculators = calcType
                                .Assembly
                                .ExportedTypes
                                .Where(x => calcType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                .Select(x => { return Activator.CreateInstance(x); })
                                .Cast<IRaidCalculator>()
                                .OrderBy(x => x.RaidNumber)
                                .ToList();
        }

        public IReadOnlyList<IRaidCalculator> GetAll()
        {
            return _RaidCalculators;
        }
    }
}
