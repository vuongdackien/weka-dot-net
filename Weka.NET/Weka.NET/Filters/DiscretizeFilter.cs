namespace Weka.NET.Filters
{
    using Weka.NET.Core;

    public class DiscretizeFilter : IFilter
    {
        public const int DefaultNumBins = 10;

        public Range<double> DiscretizeRange { get; private set; }

        public int NumBins {get; private set;}

        public double[][] CutPoints { get; private set; }

        public bool UseMDL {get; private set;}

        public bool MakeBinary { get; private set; }

        public bool UseBetterEncoding { get; private set; }

        public bool UseKononenko { get; private set; }

        public bool FindNumBins { get; private set; }

        public bool BatchFinished()
        {
            return false;
        }




    
    
    
    }
}
