namespace StorageCalc.Calculators
{
    public interface IRaidCalculator
    {
        string Name { get; }
        int RaidNumber { get; }
        RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes);
    }
}
