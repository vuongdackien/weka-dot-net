namespace Weka.NET.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class NominalAttribute : Weka.NET.Core.Attribute
    {
        readonly string[] values;

        public string[] Values { get { return values; } }

        public NominalAttribute(string name, string[] values) : base(name)
        {
            this.values = values.Clone() as string[];
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
