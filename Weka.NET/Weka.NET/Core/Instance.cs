namespace Weka.NET.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Weka.NET.Utils;
    using Weka.NET.Lang;
    using System;
    
    public sealed class Instance : IEquatable<Instance>
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

        public bool Equals(Instance other)
        {
            return Weight.Equals(other.Weight)
                && Values.ListEquals(other.Values);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != typeof(Instance))
            {
                return false;
            }

            return Equals(other as Instance);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 13 ^ Weight.GetHashCode() ^ Values.GetHashCodeForArray();
            }
        }

        public double this[int index]
        {
            get { return Values[index]; }
        }

        public bool IsMissing(int classAttributeIndex)
        {
            return double.IsNaN(Values[classAttributeIndex]);
        }

        public bool ContainsMissingValue()
        {
            return Values.Any(v => double.IsNaN(v));
        }
    }
}
