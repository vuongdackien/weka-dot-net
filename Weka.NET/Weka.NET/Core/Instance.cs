namespace Weka.NET.Core
{
    using System.Collections.Generic;
    using System.Linq;
    
    public sealed class Instance
    {
        public const double DefaultWeight = 1;

        public const double MissingValue = double.NaN;

        public IList<double> Values { get; private set; }

        public double Weight { get; private set; }

        public Instance(IEnumerable<double> values) : this(weight: DefaultWeight, values: values)
        {
        }

        public Instance(double weight, IEnumerable<double> values)
        {
            Weight = weight;
            Values = values.ToList().AsReadOnly();
        }

        public double this[int index]
        {
            get { return Values[index]; }
        }

        public bool IsMissing(int classAttributeIndex)
        {
            return double.IsNaN(Values[classAttributeIndex]);
        }
    }
}
