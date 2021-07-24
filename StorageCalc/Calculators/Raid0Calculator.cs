using StorageCalc.Resources;

namespace StorageCalc.Calculators
{
    public class Raid0Calculator : RaidCalculatorBase
    {
        public Raid0Calculator()
        {
            RaidNumber = 0;
        }

        public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
        {
            return new RaidCalculatorResult()
            {
                UseableDiskSpace = diskCount * ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes),
                FaultTolerance = LocalizedStrings.Instance["None"],
            };
        }
    }
}
