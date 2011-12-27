namespace Weka.NET.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Weka.NET.Lang;
    using Weka.NET.Utils;
    
    public sealed class NominalAttribute : Weka.NET.Core.Attribute, IEquatable<NominalAttribute>
    {
        readonly string[] values;

        public string[] Values { get { return values; } }

        public NominalAttribute(string name, string[] values) : base(name)
        {
            this.values = values.Clone() as string[];
        }

        public bool Equals(NominalAttribute other)
        {
            return base.Name.Equals(other.Name)
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

            if (other.GetType() != typeof(NominalAttribute))
            {
                return false;
            }

            return Equals(other as NominalAttribute);  
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Values.GetHashCodeForArray();
            }
        }

        public override string Decode(double value)
        {
            if (double.NaN.Equals(value))
            {
                return Core.Attribute.DecodedMissingAttribute;
            }

            return values[Convert.ToInt32(value)];
        }

        public override double Encode(string value)
        {
            if ("?".Equals(value))
            {
                return Instance.MissingValue;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Equals(value))
                {
                    return i;
                }
            }

            throw new ArgumentException("Not declared nominal value: " + value);
        }
    }
}
