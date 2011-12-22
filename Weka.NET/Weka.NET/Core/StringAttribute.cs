namespace Weka.NET.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class StringAttribute : Weka.NET.Core.Attribute
    {
        public StringAttribute(string name) : base(name)
        {
        }

        public override double Encode(string value)
        {
            throw new NotImplementedException("Not implemented");
        }

        public override string Decode(double value)
        {
            throw new NotImplementedException("Not implemented");
        }

    }
}
