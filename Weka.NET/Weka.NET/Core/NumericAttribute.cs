using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Core
{
    public class NumericAttribute : Weka.NET.Core.Attribute
    {
        public NumericAttribute(string name, int index)
            : base(name, index)
        {
        }

        public override double? Encode(string value)
        {
            return Double.Parse(value) as double?;
        }

        public override string Decode(double? value)
        {
            return value.HasValue ? value.Value.ToString() : null;
        }
    }
}
