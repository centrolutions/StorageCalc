namespace StorageCalc.Calculators
{
    public abstract class RaidCalculatorBase : IRaidCalculator
    {
        public int RaidNumber { get; protected set; }
        public string Name { get { return $"RAID {RaidNumber}"; } }

        public abstract RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes);

        protected double ConvertDiskSpaceToProperNumber(double diskSpaceTerrabytes)
        {
            var diskspaceInBytes = diskSpaceTerrabytes * 1000000000000L;
            var divisor = 1024L * 1024L * 1024L * 1024L;
            var realDiskSpace = (double)diskspaceInBytes / (double)divisor;

            return realDiskSpace;
        }

        protected RaidCalculatorResult CreateErrorResult(string errorMessage)
        {
            return new RaidCalculatorResult() { ErrorMessage = errorMessage };
        }
    }
}
