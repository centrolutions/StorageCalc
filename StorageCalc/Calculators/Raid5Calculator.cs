using StorageCalc.Resources;

namespace StorageCalc.Calculators
{
    public class Raid5Calculator : RaidCalculatorBase
    {
        public Raid5Calculator()
        {
            RaidNumber = 5;
        }

        public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
        {
            if (diskCount < 3)
                return CreateErrorResult(LocalizedStrings.Instance["AtLeastThreePlatesRequired"]);

            return new RaidCalculatorResult()
            {
                UseableDiskSpace = (diskCount - 1) * ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes),
                FaultTolerance = LocalizedStrings.Instance["OneDisk"],
            };
        }
    }
}
