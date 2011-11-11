using System.Collections.Generic;

namespace Weka.NET.Core
{
    public sealed class Instance
    {
        IList<double> Values { get; set; }

        double Weight { get; set; }
    }
}
