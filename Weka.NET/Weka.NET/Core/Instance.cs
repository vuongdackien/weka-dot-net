using System.Collections.Generic;

namespace Weka.NET.Core
{
    public sealed class Instance
    {
        public IList<double?> Values { get; set; }

        public double Weight { get; set; }

        public double? this[int index]
        {
            get { return Values[index]; }
        }


    }
}
