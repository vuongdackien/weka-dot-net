using System.Collections.Generic;
using System.Linq;

namespace Weka.NET.Core
{
    public sealed class Instance
    {
        public IList<double?> Values { get; private set; }

        public double Weight { get; private set; }

        public Instance(IEnumerable<double?> values) : this(weight: 1, values: values)
        {
            
        }

        public Instance(double weight, IEnumerable<double?> values)
        {
            Weight = weight;
            Values = values.ToList().AsReadOnly();
        }

        public double? this[int index]
        {
            get { return Values[index]; }
        }


    }
}
