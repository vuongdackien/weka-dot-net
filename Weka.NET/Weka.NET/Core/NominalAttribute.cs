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

        public NominalAttribute(string name, int index, string[] values) : base(name, index)
        {
            this.values = values.Clone() as string[];
        }

        public override string Decode(double? value)
        {
            return values[Convert.ToInt32(value.Value)];
        }

        public override double? Encode(string value)
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
