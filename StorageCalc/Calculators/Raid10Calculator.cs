using StorageCalc.Resources;

namespace StorageCalc.Calculators
{
    public class Raid10Calculator : RaidCalculatorBase
    {
        public Raid10Calculator()
        {
            RaidNumber = 10;
        }

        public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
        {
            if (diskCount % 2 != 0 || diskCount < 4)
                return CreateErrorResult(LocalizedStrings.Instance["AtLeastFourPlatesAndEvenNumber"]);

            return new RaidCalculatorResult()
            {
                UseableDiskSpace = (diskCount - 2) * ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes),
                FaultTolerance = LocalizedStrings.Instance["MinOneDisk"]
            };
        }
    }
}
