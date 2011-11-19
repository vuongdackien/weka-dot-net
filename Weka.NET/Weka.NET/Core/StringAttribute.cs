using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Core
{
    public class StringAttribute : Weka.NET.Core.Attribute
    {
        public StringAttribute(string name, int index) : base(name, index)
        {
        }

        public override double? Encode(string value)
        {
            throw new NotImplementedException("Not implemented");
        }

        public override string Decode(double? value)
        {
            throw new NotImplementedException("Not implemented");
        }

    }
}
