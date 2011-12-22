namespace Weka.NET.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class NumericAttribute : Weka.NET.Core.Attribute
    {
        public NumericAttribute(string name) : base(name)
        {
        }

        public override double Encode(string value)
        {
            return Double.Parse(value);
        }

        public override string Decode(double value)
        {
            return double.NaN.Equals(value) ? DecodedMissingAttribute : value.ToString();
        }
    }
}
